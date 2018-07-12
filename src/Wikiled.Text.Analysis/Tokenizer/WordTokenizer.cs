using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class WordTokenizer : IWordsTokenizer
    {
        private readonly IPipeline<string> pipeline;

        private readonly IWordItemFactory wordItemFactory;

        private readonly IPipeline<WordEx> wordItemPipeline;

        private readonly string[] words;

        public WordTokenizer(
            string sentence,
            IWordItemFactory wordItemFactory,
            IPipeline<string> pipeline,
            IPipeline<WordEx> wordItemPipeline,
            string[] words)
        {
            this.pipeline = pipeline ?? throw new System.ArgumentNullException(nameof(pipeline));
            this.wordItemFactory = wordItemFactory ?? throw new System.ArgumentNullException(nameof(wordItemFactory));
            this.wordItemPipeline = wordItemPipeline ?? throw new System.ArgumentNullException(nameof(wordItemPipeline));
            this.words = words ?? throw new System.ArgumentNullException(nameof(words));
            if (this.words.Length == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(words));
            }

            SentenceText = sentence;
        }

        public string SentenceText { get; }

        public IEnumerable<WordEx> GetWordItems()
        {
            return wordItemPipeline.Process(GetWords().Select(item => wordItemFactory.Construct(item)));
        }

        public IEnumerable<string> GetWords()
        {
            foreach(var word in pipeline.Process(words))
            {
                if(string.IsNullOrEmpty(word))
                {
                    continue;
                }

                yield return word;
            }
        }
    }
}
