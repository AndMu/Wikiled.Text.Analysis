using System;
using System.Collections.Generic;
using System.Linq;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class SentenceTokenizer : ISentenceTokenizer
    {
        private const string SentencePattern = @"(\S.+?[.!?])?(?=\s+|$)|.+";

        private readonly RegexSplitter splitter;

        public SentenceTokenizer(IWordsTokenizerFactory wordPipelineFactory)
        {
            splitter = new RegexSplitter(SentencePattern);
            TokenizerFactory = wordPipelineFactory ?? throw new ArgumentNullException(nameof(wordPipelineFactory));
        }

        public IWordsTokenizerFactory TokenizerFactory { get; }

        public IEnumerable<IWordsTokenizer> Parse(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            string[] sentences = splitter.Split(text).ToArray();
            string saved = string.Empty;
            for (int i = 0; i < sentences.Length; i++)
            {
                string currentSentence = sentences[i].Trim();
                while (currentSentence.Length > 1 &&
                       currentSentence[currentSentence.Length - 2] == ' ')
                {
                    currentSentence = currentSentence.Remove(currentSentence.Length - 2, 1);
                }

                if (string.IsNullOrWhiteSpace(currentSentence))
                {
                    continue;
                }

                if (i < sentences.Length - 1)
                {
                    string nextSentence = sentences[i + 1];
                    bool found = currentSentence.Count(char.IsLetterOrDigit) <= 2;
                    if (!found)
                    {
                        for (int j = 0; j < nextSentence.Length && j <= 3; j++)
                        {
                            if (nextSentence[j] == '.')
                            {
                                found = true;
                                break;
                            }
                        }
                    }

                    if (found)
                    {
                        saved += currentSentence;
                        continue;
                    }
                }

                if (!string.IsNullOrWhiteSpace(saved))
                {
                    currentSentence = saved + " " + currentSentence;
                }

                IWordsTokenizer wordsTokenizer = TokenizerFactory.Create(currentSentence);
                saved = string.Empty;
                if (wordsTokenizer != NullWordsTokenizer.Instance)
                {
                    yield return wordsTokenizer;
                }
            }
        }

        public IEnumerable<string> Split(string text)
        {
            if (string.IsNullOrWhiteSpace(text))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(text));
            }

            return Parse(text).Select(wordsTokenizer => wordsTokenizer.SentenceText);
        }
    }
}
