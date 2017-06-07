using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class NullWordsTokenizer : IWordsTokenizer
    {
        public static readonly NullWordsTokenizer Instance = new NullWordsTokenizer();

        public IEnumerable<string> GetWords()
        {
            yield break;
        }

        public IEnumerable<WordEx> GetWordItems()
        {
            yield break;
        }

        public string SentenceText => string.Empty;
    }
}
