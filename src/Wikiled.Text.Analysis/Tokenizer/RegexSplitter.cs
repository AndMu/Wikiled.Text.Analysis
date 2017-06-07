using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Wikiled.Core.Utility.Arguments;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class RegexSplitter
    {
        private readonly Regex regex;

        public RegexSplitter(string pattern)
        {
            Guard.NotNullOrEmpty(() => pattern, pattern);
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
