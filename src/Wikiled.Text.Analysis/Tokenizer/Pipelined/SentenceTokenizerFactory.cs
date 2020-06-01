using System;
using System.Collections.Generic;
using Wikiled.Text.Analysis.NLP;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.POS.Tags;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class SentenceTokenizerFactory : ISentenceTokenizerFactory
    {
        private readonly IPOSTagger tagger;

        private readonly IRawTextExtractor raw;

        public SentenceTokenizerFactory(IPOSTagger tagger, IRawTextExtractor raw)
        {
            this.tagger = tagger ?? throw new ArgumentNullException(nameof(tagger));
            this.raw = raw ?? throw new ArgumentNullException(nameof(raw));
        }

        public ISentenceTokenizer Create(bool simple, bool removeStopWords)
        {
            return Create(WordsTokenizerFactory.NotWhiteSpace, simple, removeStopWords);
        }

        public ISentenceTokenizer Create(string wordPattern, bool simple, bool removeStopWords)
        {
            List<IPipeline<WordEx>> pipelines = new List<IPipeline<WordEx>>();
            if (!simple)
            {
                pipelines.Add(new InvertorPipeline());
            }

            if (removeStopWords)
            {
                pipelines.Add(new StopWordItemPipeline());
                pipelines.Add(new WordItemFilterOutPipeline(item => item.POSType.WordType == WordType.SeparationSymbol));
                pipelines.Add(new WordItemFilterOutPipeline(item => item.IsConjunction()));
            }

            pipelines.Add(new WordItemFilterOutPipeline(item => item.POSType == SentenceFinalPunctuation.Instance));

            WordsTokenizerFactory factory = new WordsTokenizerFactory(
                wordPattern,
                new SimpleWordItemFactory(tagger, raw),
                new CombinedPipeline<string>(new LowerCasePipeline(), new WordCleanupPipeline(), new PunctuationPipeline()),
                new CombinedPipeline<WordEx>(pipelines.ToArray()));
            return new SentenceTokenizer(factory);
        }
    }
}
