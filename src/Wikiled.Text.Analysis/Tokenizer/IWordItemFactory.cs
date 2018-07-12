using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface IWordItemFactory
    {
        WordEx Construct(string word);
    }
}