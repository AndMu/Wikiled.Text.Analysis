using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Tokenizer;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class SentenceTokenizerTests
    {
        private NaivePOSTagger instance;

        [OneTimeSetUp]
        public void Setup()
        {
            instance = new NaivePOSTagger(new BNCList(), WordTypeResolver.Instance);
        }

        [TestCase("Test. Two", 2)]
        [TestCase("Test.... Two", 2)]
        [TestCase("I will go! But you please stay... Yes please", 3)]
        public void Parse(string text, int total)
        {
            var sentences = SentenceTokenizer.Create(instance, true, false)
                .Parse(text).ToArray();
            Assert.AreEqual(total, sentences.Length);
        }

        [Test]
        public void ParseShort()
        {
            var sentences = SentenceTokenizer.Create(instance, true, false)
                .Parse("quint is drawn into telling the story of his experiences aboard the u . s . s . indianapolis").ToArray();
            Assert.AreEqual(1, sentences.Length);
            Assert.AreEqual("quint is drawn into telling the story of his experiences aboard the u.s.s. indianapolis", sentences[0].SentenceText);
        }
    }
}
