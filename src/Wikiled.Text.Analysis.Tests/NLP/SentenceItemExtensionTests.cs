using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.NLP;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.NLP
{
    [TestFixture]
    public class SentenceItemExtensionTests
    {
        [Test]
        public void IsQuestion()
        {
            bool result = new SentenceItem("Test question").IsQuestion();
            ClassicAssert.IsFalse(result);
            result = new SentenceItem("Test question?").IsQuestion();
            ClassicAssert.IsTrue(result);
        }

        [Test]
        public void CountPunctuations()
        {
            int result = new SentenceItem("Test question").CountPunctuations();
            ClassicAssert.AreEqual(0, result);
            result = new SentenceItem("Test question? Test, 2, 3").CountPunctuations();
            ClassicAssert.AreEqual(3, result);
        }

        [Test]
        public void CountCharacters()
        {
            int result = new SentenceItem("Test question").CountCharacters();
            ClassicAssert.AreEqual(12, result);
            result = new SentenceItem("Test question?").CountCharacters();
            ClassicAssert.AreEqual(12, result);
        }

        [Test]
        public void CountCommas()
        {
            int result = new SentenceItem("Test question").CountCommas();
            ClassicAssert.AreEqual(0, result);
            result = new SentenceItem("Test, question?").CountCommas();
            ClassicAssert.AreEqual(1, result);
        }

        [Test]
        public void CountSemicolons()
        {
            int result = new SentenceItem("Test question").CountSemicolons();
            ClassicAssert.AreEqual(0, result);
            result = new SentenceItem("Test, question;?").CountSemicolons();
            ClassicAssert.AreEqual(1, result);
        }
    }
}
