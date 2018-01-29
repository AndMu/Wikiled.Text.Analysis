using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Core.Utility.Resources;
using Wikiled.Text.Analysis.Resources;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    public class NRCDictionary : INRCDictionary
    {
        private Dictionary<string, NRCRecord> table = new Dictionary<string, NRCRecord>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<WordNRCRecord> AllRecords => table.Select(item => new WordNRCRecord(item.Key, item.Value));
       
        public void Load()
        {
            table = new Dictionary<string, NRCRecord>(StringComparer.OrdinalIgnoreCase);
            ReadDataFromInternalStream("Resources.Dictionary.NRC.txt");
        }

        public NRCRecord FindRecord(string word)
        {
            Guard.NotNullOrEmpty(() => word, word);
            NRCRecord nrcRecord;
            table.TryGetValue(word, out nrcRecord);
            return (NRCRecord)nrcRecord?.Clone();
        }
        
        private void ReadDataFromInternalStream(string name)
        {
            using (StreamReader reader = new StreamReader(typeof(NRCDictionary).GetEmbeddedFile(name)))
            {
                Func<string, string> converterText = data => data;
                Func<string, int> converterDouble = int.Parse;
                ReadTabResourceDataFile boosterData = new ReadTabResourceDataFile(reader);
                boosterData.UseDefaultIfNotFound = false;
                int index = 0;
                foreach (var record in boosterData.ReadData(converterText, converterDouble))
                {
                    NRCRecord nrcRecord;
                    if (!table.TryGetValue(record.Item1, out nrcRecord))
                    {
                        nrcRecord = new NRCRecord(record.Item1);
                        table[record.Item1] = nrcRecord;
                        index = 0;
                    }

                    index++;
                    if (record.Item2 == 0)
                    {
                        continue;
                    }

                    switch (index)
                    {
                        case 1:
                            nrcRecord.IsAnger = true;
                            break;
                        case 2:
                            nrcRecord.IsAnticipation = true;
                            break;
                        case 3:
                            nrcRecord.IsDisgust = true;
                            break;
                        case 4:
                            nrcRecord.IsFear = true;
                            break;
                        case 5:
                            nrcRecord.IsJoy = true;
                            break;
                        case 6:
                            nrcRecord.IsNegative = true;
                            break;
                        case 7:
                            nrcRecord.IsPositive = true;
                            break;
                        case 8:
                            nrcRecord.IsSadness = true;
                            break;
                        case 9:
                            nrcRecord.IsSurprise = true;
                            break;
                        case 10:
                            nrcRecord.IsTrust = true;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException("index", index.ToString());
                    }
                }
            }
        }
    }
}
