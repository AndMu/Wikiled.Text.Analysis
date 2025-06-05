using System;
using Jitbit.Utils;
using Microsoft.Extensions.Caching.Memory;
using Wikiled.Common.Extensions;
using Wikiled.Text.Analysis.Dictionary;
using Wikiled.Text.Analysis.Extensions;

namespace Wikiled.Text.Analysis.NLP
{
    public class RawWordExtractor : IRawTextExtractor
    {
        private readonly PluralizationServiceInstance service;

        private readonly FastCache<string, string> cache = new ();

        public RawWordExtractor(IWordsDictionary dictionary)
        {
            Dictionary = dictionary ?? throw new ArgumentNullException(nameof(dictionary));
            service = new PluralizationServiceInstance();
        }

        public IWordsDictionary Dictionary { get; }

        public string GetWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return string.Empty;
            }

            return cache.GetOrAdd(
                word,
                entry => GetWordInternal(word), 
                TimeSpan.FromMinutes(1));
        }

        private string GetWordInternal(string word)
        {
            word = word.ToLower().Trim();
            if (word.Length <= 3 ||
                word == "thing")
            {
                return word;
            }

            if (word.IsEnding("nt") ||
                word.IsEnding("n't"))
            {
                return word;
            }

            if (word.Length > 4 &&
                word.IsEnding("ing"))
            {
                return RemoveIng(word);
            }

            if (word.Length > 3 &&
                word.IsEnding("ed"))
            {
                return RemoveEd(word);
            }

            var replaced = FixComparativeInternal(word);
            if (replaced != word)
            {
                return replaced;
            }

            if (service.IsPlural(word))
            {
                return service.Singularize(word);
            }

            var specialSymbol = word.IndexOf('\'');
            if (specialSymbol > 0)
            {
                word = word.Substring(0, specialSymbol);
            }

            return word;
        }

        private string RemoveEd(string word)
        {
            string original = word;
            word = word.Substring(0, word.Length - 1);
            if (Dictionary.IsKnown(word))
            {
                return word;
            }

            word = word.Substring(0, word.Length - 1);
            if (Dictionary.IsKnown(word))
            {
                return word;
            }

            word = word.Substring(0, word.Length - 1);
            if (word.Length > 2 &&
               word[word.Length - 1] == word[word.Length - 2])
            {
                word = word.Substring(0, word.Length - 1);
            }

            return Dictionary.IsKnown(word) ? word : original;
        }

        private string RemoveIng(string word)
        {
            string original = word;
            word = word.Substring(0, word.Length - 3);
            if (!word[word.Length - 1].IsVowel())
            {
                word += "e";
            }

            if (Dictionary.IsKnown(word))
            {
                return word;
            }

            word = word.Substring(0, word.Length - 1);
            if (Dictionary.IsKnown(word))
            {
                return word;
            }

            if (word.Length > 2 &&
               word[word.Length - 1] == word[word.Length - 2])
            {
                word = word.Substring(0, word.Length - 1);
            }

            return Dictionary.IsKnown(word) ? word : original;
        }

        private string FixComparativeInternal(string word)
        {
            if (word.Length < 4)
            {
                return word;
            }

            if (Dictionary.IsKnown(word))
            {
                return word;
            }

            if (word.IsEnding("er"))
            {
                return GetSingularErEst(word.Substring(0, word.Length - 2));
            }

            if (word.IsEnding("est"))
            {
                return GetSingularErEst(word.Substring(0, word.Length - 3));
            }

            return word;
        }

        private string GetSingularErEst(string word)
        {
            var original = word;
            if (word.Length < 3)
            {
                return word;
            }

            if (Dictionary.IsKnown(word))
            {
                return word;
            }

            if (word[word.Length - 1] == 'i')
            {
                word = word.Substring(0, word.Length - 1);
                word += 'y';
                if (Dictionary.IsKnown(word))
                {
                    return word;
                }
            }
            else
            {
                word += 'e';
                if (Dictionary.IsKnown(word))
                {
                    return word;
                }
            }

            return original;
        }
    }
}
