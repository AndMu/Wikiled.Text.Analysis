using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface ISentenceTokenizer
    {
        IEnumerable<IWordsTokenizer> Parse(string text);

        IEnumerable<string> Split(string text);

        IWordsTokenizerFactory TokenizerFactory { get; }
    }
}