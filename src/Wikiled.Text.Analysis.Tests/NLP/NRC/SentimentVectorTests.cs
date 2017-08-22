using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.NRC;

namespace Wikiled.Text.Analysis.Tests.NLP.NRC
{
    [TestFixture]
    public class SentimentVectorTests
    {
        [Test]
        public void Construct()
        {
            SentimentVector vector = new SentimentVector();
            Assert.AreEqual(0, vector.Anger);
            Assert.AreEqual(0, vector.Anticipation);
            Assert.AreEqual(0, vector.Disgust);
            Assert.AreEqual(0, vector.Fear);
            Assert.AreEqual(0, vector.Joy);
            Assert.AreEqual(0, vector.Trust);
            Assert.AreEqual(0, vector.Sadness);
            Assert.AreEqual(0, vector.Surprise);
            Assert.AreEqual(0, vector.Total);
            Assert.AreEqual(0, vector.TotalSum);
        }
    }
}

