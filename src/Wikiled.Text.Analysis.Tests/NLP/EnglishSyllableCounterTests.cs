using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.NLP;

namespace Wikiled.Text.Analysis.Tests.NLP
{
    [TestFixture]
    public class EnglishSyllableCounterTests
    {
        [Test]
        public void Australian()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Australian");
            ClassicAssert.AreEqual(4, total);
        }

        [Test]
        public void One()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("One");
            ClassicAssert.AreEqual(1, total);
        }

        [Test]
        public void Fucked()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Fucked");
            ClassicAssert.AreEqual(1, total);
        }

        [Test]
        public void Sentence()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("sentence");
            ClassicAssert.AreEqual(2, total);
        }

        [Test]
        public void Taken()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("taken");
            ClassicAssert.AreEqual(2, total);
        }
        

        [Test]
        public void Absolutely()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Absolutely");
            ClassicAssert.AreEqual(4, total);
        }

        [Test]
        public void Agreeable()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Agreeable");
            ClassicAssert.AreEqual(4, total);
        }

        [Test]
        public void Creature()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("creature");
            ClassicAssert.AreEqual(2, total);

            // can't handle more complicated scenarios
            total = EnglishSyllableCounter.Instance.CountSyllables("creatures");
            ClassicAssert.AreEqual(2, total);
        }
    }
}
