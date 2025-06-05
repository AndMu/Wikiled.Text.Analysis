using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;

namespace Wikiled.Text.Analysis.Tests.Tokenizer.Pipelined
{
    [TestFixture]
    public class SentenceTokenizerTests
    {
        [TestCase("Test. Two", 2)]
        [TestCase("Test.... Two", 2)]
        [TestCase("I will go! But you please stay... Yes please", 3)]
        public void Parse(string text, int total)
        {
            var sentences = Global.Factory.Create(true, false)
                .Parse(text).ToArray();
            ClassicAssert.AreEqual(total, sentences.Length);
        }

        [Test]
        public void ParseShort()
        {
            var sentences = Global.Factory.Create(true, false)
                .Parse("quint is drawn into telling the story of his experiences aboard the u . s . s . indianapolis").ToArray();
            ClassicAssert.AreEqual(1, sentences.Length);
            ClassicAssert.AreEqual("quint is drawn into telling the story of his experiences aboard the u.s.s. indianapolis", sentences[0].SentenceText);
        }
    }
}
