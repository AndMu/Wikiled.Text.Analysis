using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Wikiled.Text.Analysis.Tokenizer
{
    /// <summary>
    ///     Python port TreebankWordTokenizer
    /// </summary>
    public class TreebankWordTokenizer : ITreebankWordTokenizer
    {
        private readonly List<Regex> contraction2;

        private readonly List<Regex> contraction3;

        private readonly List<(Regex Regex, string Replacement)> endingQuotes;

        private readonly List<(Regex Regex, string Replacement)> parensBrackets;

        private readonly List<(Regex Regex, string Replacement)> punctuation;

        private readonly List<(Regex Regex, string Replacement)> startingQuotes;

        public TreebankWordTokenizer()
        {
            startingQuotes = new List<(Regex, string)> { (GetRegex("^\\\""), "``"), (GetRegex("^''"), @"``"), (GetRegex("(``)"), @" $1 "), (GetRegex("([ (\\[{<])\""), @"$1 `` ") };
            punctuation = new List<(Regex, string)>
                              {
                                  (GetRegex(@"\.\.\."), " ... "),
                                  (GetRegex(@"([?!.])\1+"), " $1"),
                                  (GetRegex(@"([:,])([^\d])"), " $1 $2"),
                                  (GetRegex(@"([:,])$"), " $1 "),
                                  (GetRegex(@"[\?\.\!]+(?=[\?\.\!])"), " $1 "),
                                  (GetRegex(@"[;@$%&]"), " ${0} "),
                                  (GetRegex("([^\\.])(\\.)([\\]\\)}>\"\\']*)\\s*$"), "$1 $2$3 "),
                                  (GetRegex(@"[?!]"), " ${0} "),
                                  (GetRegex(@"([^'])' "), "$1 ' ")
                              };

            parensBrackets = new List<(Regex, string)> { (GetRegex(@"[\]\[\(\)\{\}\<\>]"), " ${0} "), (GetRegex(@"--"), " -- ") };

            endingQuotes = new List<(Regex, string)>
                               {
                                   (GetRegex("\""), " '' "),
                                   (GetRegex(@"(\S)(\'\')"), "$1 $2 "),
                                   (GetRegex(@"([^' ])('[sS]|'[mM]|'[dD]|') "), "$1 $2 "),
                                   (GetRegex(@"([^' ])('ll|'LL|'re|'RE|'ve|'VE|n't|N'T) "), "$1 $2 ")
                               };

            // List of contractions adapted from Robert MacIntyre's tokenizer.
            contraction2 = new List<Regex>
                               {
                                   GetRegex("(?i)\b(can)(not)\b"),
                                   GetRegex("(?i)\b(d)('ye)\b"),
                                   GetRegex("(?i)\b(gim)(me)\b"),
                                   GetRegex("(?i)\b(gon)(na)\b"),
                                   GetRegex("(?i)\b(got)(ta)\b"),
                                   GetRegex("(?i)\b(lem)(me)\b"),
                                   GetRegex("(?i)\b(mor)('n)\b"),
                                   GetRegex("(?i)\b(wan)(na) ")
                               };

            contraction3 = new List<Regex> { GetRegex("(?i) ('t)(is)\b"), GetRegex("(?i) ('t)(was)\b") };
        }

        public string[] Tokenize(string text)
        {
            foreach (var quote in startingQuotes)
            {
                text = quote.Regex.Replace(text, quote.Replacement);
            }

            foreach (var quote in punctuation)
            {
                text = quote.Regex.Replace(text, quote.Replacement);
            }

            foreach (var quote in parensBrackets)
            {
                text = quote.Regex.Replace(text, quote.Replacement);
            }

            // add extra space to make things easier
            text = " " + text + " ";

            foreach (var quote in endingQuotes)
            {
                text = quote.Regex.Replace(text, quote.Replacement);
            }

            foreach (var regex in contraction2.Union(contraction3))
            {
                text = regex.Replace(text, " $1 $2 ");
            }

            return text.Split(new[] { ' ', '\n', '\t' }, StringSplitOptions.RemoveEmptyEntries);
        }

        private static Regex GetRegex(string regex)
        {
            return new Regex(regex, RegexOptions.Compiled | RegexOptions.IgnoreCase);
        }
    }
}
