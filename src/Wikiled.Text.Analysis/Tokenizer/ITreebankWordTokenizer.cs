namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface ITreebankWordTokenizer
    {
        string[] Tokenize(string text);
    }
}