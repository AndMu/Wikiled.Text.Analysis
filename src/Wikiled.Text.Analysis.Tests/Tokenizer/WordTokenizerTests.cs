using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Tokenizer;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class WordTokenizerTests
    {
        private NaivePOSTagger instance;

        [OneTimeSetUp]
        public void Setup()
        {
            instance = new NaivePOSTagger(new BNCList(), WordTypeResolver.Instance);
        }

        [Test]
        public void GetWords()
        {
            WordTokenizer tokenizer = new WordTokenizer(
                "Test", 
                new SimpleWordItemFactory(instance), 
                NullSimpleWordPipeline.Instance, 
                NullWordItemPipeline.Instance, 
                new[] { "test", "one" });
            var result = tokenizer.GetWords().ToArray();
            Assert.AreEqual("Test", tokenizer.SentenceText);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("test", result[0]);
            Assert.AreEqual("one", result[1]);
        }

        [Test]
        public void GetWordsWithFilterOut()
        {
            WordTokenizer tokenizer = new WordTokenizer(
                "Test",
                new SimpleWordItemFactory(instance), 
                NullSimpleWordPipeline.Instance, 
                NullWordItemPipeline.Instance, 
                new[] { "", "one" });
            var result = tokenizer.GetWords().ToArray();
            Assert.AreEqual(1, result.Length);
            Assert.AreEqual("one", result[0]);
        }
    }
}
