using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface IWordsTokenizerFactory
    {
        IWordsTokenizer Create(string sentence);

        CombinedPipeline<WordEx> WordItemPipeline { get; }

        CombinedPipeline<string> Pipeline { get; }
    }
}
