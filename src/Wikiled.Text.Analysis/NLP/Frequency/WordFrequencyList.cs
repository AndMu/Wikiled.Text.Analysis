using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Text.Analysis.Dictionary;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class WordFrequencyList : IWordFrequencyList
    {
        private readonly Dictionary<string, FrequencyInformation> indexTable = new Dictionary<string, FrequencyInformation>(StringComparer.OrdinalIgnoreCase);

        public WordFrequencyList(string name, string fileName)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(fileName))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(fileName));
            }

            Name = name;
            var dictionary = WordsDictionary.Construct(new CompressedDictionaryStream(fileName, new EmbeddedStreamSource<WordsDictionary>()));
            int index = 0;
            foreach (var item in dictionary.RawData.OrderByDescending(item => item.Value))
            {
                index++;
                indexTable[item.Key] = new FrequencyInformation(item.Key, index, item.Value);
            }
        }

        public IEnumerable<FrequencyInformation> All => indexTable.Values;

        public string Name { get; }

        public FrequencyInformation GetIndex(string word)
        {
            return !indexTable.TryGetValue(word, out var index) ? null : index;
        }
    }
}
