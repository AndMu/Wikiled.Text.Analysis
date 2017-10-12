using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wikiled.Core.Utility.Helpers;
using Wikiled.Core.Utility.Resources;
using Wikiled.Core.Utility.Resources.Wikiled.Core.Definitions;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class BNCList : IWordFrequencyList, IPosTagResolver
    {
        private readonly Dictionary<string, FrequencyInformation> indexTable = new Dictionary<string, FrequencyInformation>(StringComparer.OrdinalIgnoreCase);

        public BNCList()
        {
            ReadData();
        }

        public string Name => "BNC List";

        public FrequencyInformation GetIndex(string word)
        {
            return !indexTable.TryGetValue(word, out var index) ? null : index;
        }

        public IEnumerable<FrequencyInformation> All => indexTable.Values;

        public BasePOSType GetPOS(string word)
        {
            return !indexTable.TryGetValue(word, out var index) ? POSTags.Instance.UnknownWord : index.Pos;
        }

        private void ReadData()
        {
            int lineId = 0;
            Dictionary<string, (double Frequency, BasePOSType Pos)> table = new Dictionary<string, (double, BasePOSType)>(StringComparer.OrdinalIgnoreCase);
            using (BinaryReader reader = new BinaryReader(typeof(BNCList).GetEmbeddedFile(@"Resources.Frequency.bnc.dat")))
            {
                byte[] data = new byte[reader.BaseStream.Length];
                reader.Read(data, 0, data.Length);
                var unzipedText = data.UnZipString();

                foreach (var line in unzipedText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    lineId++;
                    if (string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var entries = line.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                    if (entries.Length != 3)
                    {
                        throw new ResourcesException($"Failed reading file on line: {lineId}");
                    }

                    try
                    {
                        string word = string.Intern(entries[0].Trim());
                        table[word] = (double.Parse(entries[1]), POSTags.Instance.FindType(entries[2]));
                    }
                    catch (Exception ex)
                    {
                        throw new ResourcesException($"Failed reading file on line: {lineId}", ex);
                    }
                }
            }

            int index = 0;
            foreach (var item in table.OrderByDescending(item => item.Value.Item1))
            {
                index++;
                var record = table[item.Key];
                indexTable[item.Key] = new FrequencyInformation(item.Key, index, record.Frequency, record.Pos);
            }
        }
    }
}
