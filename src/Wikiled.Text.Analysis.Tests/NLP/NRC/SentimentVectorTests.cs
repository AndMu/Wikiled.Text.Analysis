using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(0, vector.Anger);
            ClassicAssert.AreEqual(0, vector.Anticipation);
            ClassicAssert.AreEqual(0, vector.Disgust);
            ClassicAssert.AreEqual(0, vector.Fear);
            ClassicAssert.AreEqual(0, vector.Joy);
            ClassicAssert.AreEqual(0, vector.Trust);
            ClassicAssert.AreEqual(0, vector.Sadness);
            ClassicAssert.AreEqual(0, vector.Surprise);
            ClassicAssert.AreEqual(0, vector.Total);
            ClassicAssert.AreEqual(0, vector.TotalSum);
        }
    }
}

