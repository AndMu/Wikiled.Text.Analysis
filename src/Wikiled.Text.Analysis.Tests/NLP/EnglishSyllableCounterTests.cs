using NUnit.Framework;
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
            Assert.AreEqual(4, total);
        }

        [Test]
        public void One()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("One");
            Assert.AreEqual(1, total);
        }

        [Test]
        public void Fucked()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Fucked");
            Assert.AreEqual(1, total);
        }

        [Test]
        public void Sentence()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("sentence");
            Assert.AreEqual(2, total);
        }

        [Test]
        public void Taken()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("taken");
            Assert.AreEqual(2, total);
        }
        

        [Test]
        public void Absolutely()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Absolutely");
            Assert.AreEqual(4, total);
        }

        [Test]
        public void Agreeable()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("Agreeable");
            Assert.AreEqual(4, total);
        }

        [Test]
        public void Creature()
        {
            var total = EnglishSyllableCounter.Instance.CountSyllables("creature");
            Assert.AreEqual(2, total);

            // can't handle more complicated scenarios
            total = EnglishSyllableCounter.Instance.CountSyllables("creatures");
            Assert.AreEqual(2, total);
        }
    }
}
