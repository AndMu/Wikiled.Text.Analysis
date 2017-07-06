using Wikiled.Text.Analysis.Dictionary;

namespace Wikiled.Text.Analysis.NLP
{
    public interface IRawTextExtractor
    {
        IWordsDictionary Dictionary { get; }

        string GetWord(string word);
    }
}
