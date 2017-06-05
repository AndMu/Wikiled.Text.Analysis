using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface IWordsTokenizer
    {
        IEnumerable<string> GetWords();

        IEnumerable<WordEx> GetWordItems();

        string SentenceText { get; }
    }
}
