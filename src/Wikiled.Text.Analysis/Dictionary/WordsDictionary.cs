using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.Dictionary
{
    public class WordsDictionary
    {
        private WordsDictionary(Dictionary<string, double> table)
        {
            RawData = table ?? throw new ArgumentNullException(nameof(table));
        }

        public Dictionary<string, double> RawData { get; }

        public static WordsDictionary Construct(IDictionaryStream stream)
        {
            if (stream == null)
            {
                throw new ArgumentNullException(nameof(stream));
            }

            return new WordsDictionary(stream.ReadDataFromStream(double.Parse).ToDictionary(item => item.Word, item => item.Value, StringComparer.OrdinalIgnoreCase));
        }

        public bool Contains(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException("message", nameof(word));
            }

            return RawData.ContainsKey(word);
        }
    }
}
