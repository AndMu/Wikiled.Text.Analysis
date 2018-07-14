using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Text.Analysis.Dictionary;
using Wikiled.Text.Analysis.Dictionary.Streams;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    public class NRCDictionary : INRCDictionary
    {
        private bool loaded;

        private Dictionary<string, NRCRecord> table = new Dictionary<string, NRCRecord>(StringComparer.OrdinalIgnoreCase);

        public IEnumerable<WordNRCRecord> AllRecords => table.Select(item => new WordNRCRecord(item.Key, item.Value));

        public void Load(IDictionaryStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            table = new Dictionary<string, NRCRecord>(StringComparer.OrdinalIgnoreCase);
            ReadDataFromInternalStream(stream);
        }

        public void Load()
        {
            loaded = true;
            var stream = new CompressedDictionaryStream("Resources.Dictionary.NRC.dat", new EmbeddedStreamSource<WordsDictionary>());
            Load(stream);
        }

        public SentimentVector Extract(IEnumerable<WordEx> words)
        {
            if (words is null)
            {
                throw new ArgumentNullException(nameof(words));
            }

            if (!loaded)
            {
                throw new InvalidOperationException("Not loaded");
            }

            var vector = new SentimentVector();
            foreach (var word in words)
            {
                vector.ExtractData(FindRecord(word));
            }

            return vector;
        }

        public NRCRecord FindRecord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(word));
            }

            if (!loaded)
            {
                throw new InvalidOperationException("Not loaded");
            }

            table.TryGetValue(word, out NRCRecord nrcRecord);
            return (NRCRecord)nrcRecord?.Clone();
        }

        public NRCRecord FindRecord(WordEx word)
        {
            if (!loaded)
            {
                throw new InvalidOperationException("Not loaded");
            }

            NRCRecord nrcRecord = null;
            foreach (var text in word.GetPossibleText())
            {
                nrcRecord = FindRecord(text);
                if (nrcRecord != null)
                {
                    break;
                }
            }

            if (nrcRecord == null)
            {
                return null;
            }

            nrcRecord = (NRCRecord)nrcRecord.Clone();
            if (word.IsInverted)
            {
                nrcRecord.Invert();
            }

            return nrcRecord;
        }

        private void ReadDataFromInternalStream(IDictionaryStream stream)
        {
            int index = 0;
            foreach (var record in stream.ReadDataFromStream(int.Parse))
            {
                if (!table.TryGetValue(record.Item1, out var nrcRecord))
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
