using System;
using System.Collections.Generic;
using System.IO;
using Wikiled.Common.Resources;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class ReadTabResourceDataFile : IDisposable
    {
        private readonly TextReader reader;

        private readonly string name;

        public ReadTabResourceDataFile(string name, TextReader stream)
        {
            reader = stream ?? throw new ArgumentNullException(nameof(stream));
            this.name = name;
        }

        public bool UseDefaultIfNotFound { get; set; }

        public char Separator { get; set; } = '\t';

        public void Dispose()
        {
            reader?.Dispose();
        }

        public IEnumerable<(T1 Word, T2 Value)> ReadData<T1, T2>(Func<string, T1> coverver1, Func<string, T2> coverver2)
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

                var entries = line.Split(new[] { Separator }, StringSplitOptions.RemoveEmptyEntries);
                string word;
                try
                {
                    word = string.Intern(entries[0].Trim());
                }
                catch (Exception ex)
                {
                    throw new ResourcesException($"Failed reading file {name} on line: {lineId}", ex);
                }

                yield return (
                    coverver1(word),
                    entries.Length < 2 && UseDefaultIfNotFound
                        ? default(T2)
                        : coverver2(entries[1].Trim()));
            }
        }

        public IEnumerable<(T1 Word, T2 Value)> ReadData<T1, T2>()
        {
            return ReadData(data => (T1)Convert.ChangeType(data, typeof(T1)), data => (T2)Convert.ChangeType(data, typeof(T2)));
        }
    }
}
