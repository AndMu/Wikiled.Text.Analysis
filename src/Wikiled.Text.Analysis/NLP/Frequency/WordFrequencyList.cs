using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Text.Analysis.Dictionary;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class WordFrequencyList : IWordFrequencyList
    {
        private readonly Dictionary<string, int> indexTable = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        public WordFrequencyList(string name, string fileName)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNullOrEmpty(() => fileName, fileName);
            Name = name;
            var dictionary = WordsDictionary.ConstructFromInternalZippedStream(fileName);
            int index = 0;
            foreach (var item in dictionary.RawData.OrderByDescending(item => item.Value))
            {
                index++;
                indexTable[string.Intern(item.Key)] = index;
            }
        }

        public int GetIndex(string word)
        {
            int index;
            if (!indexTable.TryGetValue(word, out index))
            {
                return -1;
            }

            return index;
        }

        public string Name { get; }
    }
}
