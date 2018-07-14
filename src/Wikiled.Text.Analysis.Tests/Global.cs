using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using Wikiled.Text.Analysis.Dictionary;
using Wikiled.Text.Analysis.NLP;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Tests
{
    [SetUpFixture]
    public class Global
    {
        public static NaivePOSTagger PosTagger { get; private set; }

        public static IRawTextExtractor Raw { get; private set; }

        public static SentenceTokenizerFactory Factory { get; private set; }

        [OneTimeSetUp]
        public void Setup()
        {
            PosTagger = new NaivePOSTagger(new BNCList(), WordTypeResolver.Instance);
            Raw = new RawWordExtractor(new BasicEnglishDictionary(), new MemoryCache(new MemoryCacheOptions()));
            Factory = new SentenceTokenizerFactory(PosTagger, Raw);
        }

        [OneTimeTearDown]
        public void Clean()
        {
        }
    }
}
