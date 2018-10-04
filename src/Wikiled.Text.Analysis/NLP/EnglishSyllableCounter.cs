using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wikiled.Text.Analysis.NLP
{
    public class EnglishSyllableCounter
    {
        public static readonly EnglishSyllableCounter Instance = new EnglishSyllableCounter();

        private readonly Regex wowelSplit = new Regex("[^aeiouy]+", RegexOptions.Compiled | RegexOptions.IgnoreCase);

        private readonly Regex[] subtractSyllables =
            {
                    new Regex("cial", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("tia", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("cius", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("cious", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("giu", RegexOptions.Compiled | RegexOptions.IgnoreCase), // belgium!
                    new Regex("ion", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("iou", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("sia$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex(".ely$", RegexOptions.Compiled | RegexOptions.IgnoreCase) // absolutely! (but not ely!)
                };

        private readonly Regex[] addSyllables =
            {
                    new Regex("ia", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("riet", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("dien", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("iu", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("io", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("ii", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("[aeiouym]bl$", RegexOptions.Compiled | RegexOptions.IgnoreCase), // -Vble, plus -mble
                    new Regex("[aeiou]{3}", RegexOptions.Compiled | RegexOptions.IgnoreCase), // agreeable
                    new Regex("^mc", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("ism$", RegexOptions.Compiled | RegexOptions.IgnoreCase), // -isms 
                    new Regex(@"([^aeiouy])\1l$", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    // middle twiddle battle bottle, etc.
                    new Regex("[^l]lien", RegexOptions.Compiled | RegexOptions.IgnoreCase), // alien, salient [1]
                    new Regex("^coa[dglx].", RegexOptions.Compiled | RegexOptions.IgnoreCase), // [2]
                    new Regex("[^gq]ua[^auieo]", RegexOptions.Compiled | RegexOptions.IgnoreCase),
                    new Regex("dnt$", RegexOptions.Compiled | RegexOptions.IgnoreCase) // couldn't
                };

        private EnglishSyllableCounter()
        {
        }

        public int CountSyllables(string word)
        {
            int result = 0;

            word = word.Trim();
            if (string.IsNullOrWhiteSpace(word))
            {
                return result;
            }
            if (word.Length == 1)
            {
                return 1;
            }

            word = word.Replace("'", "");
            if (word.EndsWith("e") && !word.EndsWith("le"))
            {
                word = word.Substring(0, word.Length - 1);

            }
            if (word.Length > 2 &&
                (word.EndsWith("es") || word.EndsWith("ed")))
            {
                word = word.Substring(0, word.Length - 2);
            }

            String[] vowelGroups = wowelSplit.Split(word);

            //	Handle special cases.

            //	Subtract from syllable count
            //	for these patterns.

            foreach (var subtractSyllable in subtractSyllables)
            {
                if (subtractSyllable.IsMatch(word))
                {
                    result--;
                }
            }
            //	Add to syllable count for these patterns.
            foreach (var subtractSyllable in addSyllables)
            {
                if (subtractSyllable.IsMatch(word))
                {
                    result++;
                }
            }

            //	Count vowel groupings.

            result += vowelGroups.Count(item => item.Length > 0);

            //	Return syllable count of
            //	at least one.

            return Math.Max(result, 1);
        }
    }
}
