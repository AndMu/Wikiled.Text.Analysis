using System;
using System.Collections.Generic;
using System.IO;
using NLog;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Core.Utility.Resources.Wikiled.Core.Definitions;

namespace Wikiled.Text.Analysis.Resources
{
    public class ReadTabResourceDataFile : IDisposable
    {
        private readonly string file;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        private readonly TextReader reader;

        public ReadTabResourceDataFile(string file)
        {
            Guard.NotNullOrEmpty(() => file, file);
            this.file = file;
            reader = File.OpenText(file);
        }

        public ReadTabResourceDataFile(TextReader stream)
        {
            Guard.NotNull(() => stream, stream);
            reader = stream;
        }

        public bool UseDefaultIfNotFound { get; set; }

        public static Dictionary<string, double> ReadTextData(string file, bool useDefault)
        {
            Guard.NotNullOrEmpty(() => file, file);
            using (var reader = new ReadTabResourceDataFile(file))
            {
                return reader.ReadTextData(useDefault);
            }
        }

        public void Dispose()
        {
            reader?.Dispose();
        }

        public IEnumerable<Tuple<T1, T2>> ReadData<T1, T2>(Func<string, T1> coverver1, Func<string, T2> coverver2)
        {
            string line;
            var lineId = 0;
            while ((line = reader.ReadLine()) != null)
            {
                lineId++;
                if (string.IsNullOrEmpty(line))
                {
                    continue;
                }

                var entries = line.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                var word = string.Empty;
                try
                {
                    word = string.Intern(entries[0].Trim());
                }
                catch (Exception ex)
                {
                    throw new ResourcesException($"Failed reading file {file} on line: {lineId}", ex);
                }

                yield return new Tuple<T1, T2>(
                    coverver1(word),
                    entries.Length < 2 && UseDefaultIfNotFound
                        ? default(T2)
                        : coverver2(entries[1].Trim()));
            }
        }

        public Dictionary<T1, T2> ReadData<T1, T2>(IEqualityComparer<T1> comparer)
        {
            return ReadDataSafeDictionary(
                data => (T1)Convert.ChangeType(data, typeof(T1)),
                data => (T2)Convert.ChangeType(data, typeof(T2)),
                comparer);
        }

        public Dictionary<T1, T2> ReadDataSafeDictionary<T1, T2>(Func<string, T1> coverver1, Func<string, T2> coverver2, IEqualityComparer<T1> comparer)
        {
            var table = new Dictionary<T1, T2>(comparer);
            foreach (var record in ReadData(coverver1, coverver2))
            {
                if (table.ContainsKey(record.Item1))
                {
                    log.Warn("Record <{0}> already exist - replacing...", record.Item1);
                }

                table[record.Item1] = record.Item2;
            }

            return table;
        }

        public Dictionary<string, double> ReadTextData(bool useDefault)
        {
            Func<string, string> coverterText = data => data;
            Func<string, double> coverterDouble = double.Parse;
            using (var boosterData = new ReadTabResourceDataFile(reader))
            {
                boosterData.UseDefaultIfNotFound = useDefault;
                return boosterData.ReadDataSafeDictionary(coverterText, coverterDouble, StringComparer.OrdinalIgnoreCase);
            }
        }
    }
}
