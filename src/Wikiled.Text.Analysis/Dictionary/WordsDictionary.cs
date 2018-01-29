using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.Dictionary
{
    public class WordsDictionary
    {
        private WordsDictionary(Dictionary<string, double> table)
        {
            Guard.NotNull(() => table, table);
            RawData = table;
        }

        public Dictionary<string, double> RawData { get; }

        public static WordsDictionary Construct(IDictionaryStream stream)
        {
            Guard.NotNull(() => stream, stream);
            return new WordsDictionary(stream.ReadDataFromStream(double.Parse).ToDictionary(item => item.Word, item => item.Value, StringComparer.OrdinalIgnoreCase));
        }

        public bool Contains(string word)
        {
            Guard.NotNullOrEmpty(() => word, word);
            return RawData.ContainsKey(word);
        }
    }
}
