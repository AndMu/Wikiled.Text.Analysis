namespace Wikiled.Text.Analysis.Dictionary
{
    public interface IWordsDictionary
    {
        bool IsKnown(string word);

        string[] GetWords();
    }
}
