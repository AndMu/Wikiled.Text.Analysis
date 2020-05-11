using System;
using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.NLP
{
    public static class NGramExtension
    {
        public static IEnumerable<NGramBlock> GetNearNGram(this WordEx[] words, int index, int length)
        {
            if (words is null)
            {
                throw new ArgumentNullException(nameof(words));
            }

            if (length <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(length));
            }

            if (index <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }

            if (words.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(words));
            }

            int startingIndex = index - (length - 1);
            int endIndex = index + (length - 1);
            startingIndex = startingIndex < 0 ? 0 : startingIndex;
            endIndex = endIndex < words.Length ? endIndex : words.Length - 1;
            WordEx[] subset = new WordEx[endIndex - startingIndex + 1];
            for (int i = startingIndex; i <= endIndex; i++)
            {
                subset[i - startingIndex] = words[i];
            }

            return GetNGram(subset, length);
        }

        public static IEnumerable<NGramBlock> GetNGram(this WordEx[] words, int length = 3)
        {
            if (words.Length < length)
            {
                yield break;
            }

            var wordOccurrences = new Queue<WordEx>();

            foreach (var word in words)
            {
                wordOccurrences.Enqueue(word);
                if (wordOccurrences.Count == length)
                {
                    yield return new NGramBlock(wordOccurrences.ToArray());
                    wordOccurrences.Dequeue();
                }
            }
        }
    }
}