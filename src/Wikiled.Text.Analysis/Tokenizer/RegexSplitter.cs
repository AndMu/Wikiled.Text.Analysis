using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class RegexSplitter
    {
        private readonly Regex regex;

        public RegexSplitter(string pattern)
        {
            if (string.IsNullOrEmpty(pattern))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(pattern));
            }

            regex = new Regex(pattern, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }

        public IEnumerable<string> Split(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                yield break;
            }

            var words = from word in regex.Matches(text).Cast<Match>()
                        select word.Value;

            foreach (var word in words)
            {
                if (!string.IsNullOrWhiteSpace(word))
                {
                    yield return word;
                }
            }
        }
    }
}
