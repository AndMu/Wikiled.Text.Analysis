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
        private readonly Dictionary<string, Tuple<int, BasePOSType>> indexTable = new Dictionary<string, Tuple<int, BasePOSType>>(StringComparer.OrdinalIgnoreCase);

        public BNCList()
        {
            ReadData();
        }

        public string Name => "BNC List";

        public int GetIndex(string word)
        {
            Tuple<int, BasePOSType> index;
            if(!indexTable.TryGetValue(word, out index))
            {
                return -1;
            }

            return index.Item1;
        }

        public BasePOSType GetPOS(string word)
        {
            Tuple<int, BasePOSType> index;
            return !indexTable.TryGetValue(word, out index) ? POSTags.Instance.UnknownWord : index.Item2;
        }

        private void ReadData()
        {
            int lineId = 0;
            Dictionary<string, Tuple<double, BasePOSType>> table = new Dictionary<string, Tuple<double, BasePOSType>>(StringComparer.OrdinalIgnoreCase);
            using(BinaryReader reader = new BinaryReader(typeof(BNCList).GetEmbeddedFile(@"Resources.Frequency.bnc.dat")))
            {
                byte[] data = new byte[reader.BaseStream.Length];
                reader.Read(data, 0, data.Length);
                var unzipedText = data.UnZipString();

                foreach(var line in unzipedText.Split(new[] { "\r\n" }, StringSplitOptions.RemoveEmptyEntries))
                {
                    lineId++;
                    if(string.IsNullOrWhiteSpace(line))
                    {
                        continue;
                    }

                    var entries = line.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries);
                    if(entries.Length != 3)
                    {
                        throw new ResourcesException($"Failed reading file on line: {lineId}");
                    }

                    try
                    {
                        string word = string.Intern(entries[0].Trim());
                        table[word] = new Tuple<double, BasePOSType>(double.Parse(entries[1]), POSTags.Instance.FindType(entries[2]));
                    }
                    catch(Exception ex)
                    {
                        throw new ResourcesException($"Failed reading file on line: {lineId}", ex);
                    }
                }
            }

            int index = 0;
            foreach(var item in table.OrderByDescending(item => item.Value.Item1))
            {
                index++;
                indexTable[string.Intern(item.Key)] = new Tuple<int, BasePOSType>(index, table[item.Key].Item2);
            }
        }
    }
}
