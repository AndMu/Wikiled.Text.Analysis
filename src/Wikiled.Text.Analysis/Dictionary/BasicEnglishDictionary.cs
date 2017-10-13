using System.Linq;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.Dictionary
{
    public class BasicEnglishDictionary : IWordsDictionary
    {
        private readonly WordsDictionary words;

        private BasicEnglishDictionary()
        {
            words = WordsDictionary.Construct(new CompressedDictionaryStream(@"Resources.Dictionary.RawEnglish.dat", new InternalStreamSource()));
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
