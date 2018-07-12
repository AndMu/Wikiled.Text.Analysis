using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Tokenizer;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class WordsTokenizerFactoryTests
    {
        private NaivePOSTagger instance;

        [OneTimeSetUp]
        public void Setup()
        {
            instance = new NaivePOSTagger(new BNCList(), WordTypeResolver.Instance);
        }

        [Test]
        public void Create()
        {
            WordsTokenizerFactory tokenizerFactory = new WordsTokenizerFactory(
                WordsTokenizerFactory.Grouped,
                new SimpleWordItemFactory(instance),
                new CombinedPipeline<string>(),
                new CombinedPipeline<WordEx>());
            IWordsTokenizer tokenizer = tokenizerFactory.Create("Test words");
            string[] words = tokenizer.GetWords().ToArray();
            Assert.AreEqual(2, words.Length);
            Assert.AreEqual("Test", words[0]);
            Assert.AreEqual("words", words[1]);
        }

        [Test]
        public void CreateNull()
        {
            WordsTokenizerFactory tokenizerFactory = new WordsTokenizerFactory(
                WordsTokenizerFactory.Grouped,
                new SimpleWordItemFactory(instance),
               new CombinedPipeline<string>(),
                new CombinedPipeline<WordEx>());
            IWordsTokenizer tokenizer = tokenizerFactory.Create(null);
            Assert.IsInstanceOf<NullWordsTokenizer>(tokenizer);
        }
    }
}
