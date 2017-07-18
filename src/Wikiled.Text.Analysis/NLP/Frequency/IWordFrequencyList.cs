namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public interface IWordFrequencyList
    {
        int GetIndex(string word);

        string Name { get; }
    }
}