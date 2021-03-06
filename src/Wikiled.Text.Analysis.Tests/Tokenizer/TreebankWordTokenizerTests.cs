using NUnit.Framework;
using Wikiled.Text.Analysis.Tokenizer;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class TreebankWordTokenizerTests
    {
        private TreebankWordTokenizer instance;

        [SetUp]
        public void Setup()
        {
            instance = CreateTreebankWordTokenizer();
        }

        [Test]
        public void DoubleEnd()
        {
            var text = "By default, the application is set to search for new virus definitions daily, but you always can use the scheduling tool to change this..";
            var result = instance.Tokenize(text);
            Assert.AreEqual(27, result.Length);
        }

        [Test]
        public void BasicTokenize()
        {
            var text = "''Good muffins cost $3.88\nin New York.  Please buy me\ntwo of them.\nThanks.''";
            var result = instance.Tokenize(text);
            Assert.AreEqual(18, result.Length);
            Assert.AreEqual("Good", result[1]);
            Assert.AreEqual("buy", result[10]);

            text = "They'll save and invest more.";
            result = instance.Tokenize(text);
            Assert.AreEqual(7, result.Length);
            Assert.AreEqual("'ll", result[1]);

            text = "hi, my name can't hello,";
            result = instance.Tokenize(text);
            Assert.AreEqual(8, result.Length);
            Assert.AreEqual(",", result[1]);

            text = "hi #mario";
            result = instance.Tokenize(text);
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("#mario", result[1]);
        }

        private TreebankWordTokenizer CreateTreebankWordTokenizer()
        {
            return TreebankWordTokenizer.Tokenizer;
        }
    }
}
