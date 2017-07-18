using System.Linq;

namespace Wikiled.Text.Analysis.Dictionary
{
    public class BasicEnglishDictionary : IWordsDictionary
    {
        private readonly WordsDictionary words;

        private BasicEnglishDictionary()
        {
            words = WordsDictionary.ConstructFromInternalZippedStream(@"Resources.Dictionary.RawEnglish.dat");
        }

        public static BasicEnglishDictionary Instance { get; } = new BasicEnglishDictionary();

        public string[] GetWords()
        {
            return words.RawData.Keys.ToArray();
        }

        public bool IsKnown(string word)
        {
            return words.Contains(word);
        }
    }
}
