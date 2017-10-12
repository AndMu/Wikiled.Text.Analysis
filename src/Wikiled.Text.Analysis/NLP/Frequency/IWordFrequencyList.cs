using System.Collections.Generic;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public interface IWordFrequencyList
    {
        FrequencyInformation GetIndex(string word);

        IEnumerable<FrequencyInformation> All { get; }

        string Name { get; }
    }
}