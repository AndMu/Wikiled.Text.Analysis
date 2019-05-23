using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure
{
    public class BagOfWords : IBagOfWords
    {
        private readonly List<WordEx> words = new List<WordEx>();

        public static BagOfWords Create(params string[] words)
        {
            var bag = new BagOfWords();
            foreach (var word in words)
            {
                bag.words.Add(new WordEx(word));
            }

            return bag;
        }

        public int TotalWords => words.Count;

        public IEnumerable<WordEx> Words => words;
    }
}
