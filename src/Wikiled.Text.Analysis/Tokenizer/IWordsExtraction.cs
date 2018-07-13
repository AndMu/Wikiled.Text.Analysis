using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface IWordsExtraction
    {
        Document GetDocument(string text);
    }
}