using System;
using Wikiled.Text.Analysis.NLP;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class SimpleWordItemFactory : IWordItemFactory
    {
        private readonly IPOSTagger tagger;

        private readonly IRawTextExtractor raw;

        public SimpleWordItemFactory(IPOSTagger tagger, IRawTextExtractor raw)
        {
            this.tagger = tagger ?? throw new ArgumentNullException(nameof(tagger));
            this.raw = raw ?? throw new ArgumentNullException(nameof(raw));
        }

        public WordEx Construct(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(word));
            }

            var wordEx = new WordEx(word);
            wordEx.Type = tagger.GetTag(word).Tag;
            wordEx.IsStop = Words.WordTypeResolver.Instance.IsStop(word);
            wordEx.IsInvertor = Words.WordTypeResolver.Instance.IsInvertor(word);
            wordEx.Raw = raw.GetWord(word);
            return wordEx;
        }
    }
}
