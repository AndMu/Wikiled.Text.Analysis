using System;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class SimpleWordItemFactory : IWordItemFactory
    {
        private readonly IPOSTagger tagger;

        public SimpleWordItemFactory(IPOSTagger tagger)
        {
            this.tagger = tagger ?? throw new ArgumentNullException(nameof(tagger));
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
            return wordEx;
        }
    }
}
