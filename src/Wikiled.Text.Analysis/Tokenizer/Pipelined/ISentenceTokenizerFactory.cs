namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public interface ISentenceTokenizerFactory
    {
        ISentenceTokenizer Create(bool simple, bool removeStopWords);
        ISentenceTokenizer Create(string wordPattern, bool simple, bool removeStopWords);
    }
}