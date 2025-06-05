using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(27, result.Length);
        }

        [Test]
        public void BasicTokenize()
        {
            var text = "''Good muffins cost $3.88\nin New York.  Please buy me\ntwo of them.\nThanks.''";
            var result = instance.Tokenize(text);
            ClassicAssert.AreEqual(18, result.Length);
            ClassicAssert.AreEqual("Good", result[1]);
            ClassicAssert.AreEqual("buy", result[10]);

            text = "They'll save and invest more.";
            result = instance.Tokenize(text);
            ClassicAssert.AreEqual(7, result.Length);
            ClassicAssert.AreEqual("'ll", result[1]);

            text = "hi, my name can't hello,";
            result = instance.Tokenize(text);
            ClassicAssert.AreEqual(8, result.Length);
            ClassicAssert.AreEqual(",", result[1]);

            text = "hi #mario";
            result = instance.Tokenize(text);
            ClassicAssert.AreEqual(2, result.Length);
            ClassicAssert.AreEqual("#mario", result[1]);
        }

        private TreebankWordTokenizer CreateTreebankWordTokenizer()
        {
            return TreebankWordTokenizer.Tokenizer;
        }
    }
}
