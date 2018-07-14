using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.NRC;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.NLP.NRC
{
    [TestFixture]
    public class NRCDictionaryTests
    {
        private readonly NRCDictionary dictionary;

        public NRCDictionaryTests()
        {
            dictionary = new NRCDictionary();
            dictionary.Load();
        }

        [Test]
        public void Extract()
        {
            var vector = dictionary.Extract(new[] { new WordEx("kill") });
            Assert.AreEqual(0, vector.Anger);
            Assert.AreEqual(0, vector.Anticipation);
            Assert.AreEqual(0, vector.Disgust);
            Assert.AreEqual(1, vector.Fear);
            Assert.AreEqual(0, vector.Joy);
            Assert.AreEqual(0, vector.Trust);
            Assert.AreEqual(1, vector.Sadness);
            Assert.AreEqual(0, vector.Surprise);
            Assert.AreEqual(1, vector.Total);
            Assert.AreEqual(2, vector.TotalSum);

            vector = dictionary.Extract(new[] { new WordEx(("love")) });
            Assert.AreEqual(0, vector.Anger);
            Assert.AreEqual(0, vector.Anticipation);
            Assert.AreEqual(0, vector.Disgust);
            Assert.AreEqual(0, vector.Fear);
            Assert.AreEqual(1, vector.Joy);
            Assert.AreEqual(0, vector.Sadness);
            Assert.AreEqual(0, vector.Surprise);
            Assert.AreEqual(0, vector.Trust);
            Assert.AreEqual(1, vector.Total);
            Assert.AreEqual(1, vector.TotalSum);
        }

        [Test]
        public void FindRecord()
        {
            var record = dictionary.FindRecord("smut");
            Assert.IsFalse(record.IsAnger);
            Assert.IsFalse(record.IsAnticipation);
            Assert.IsTrue(record.IsDisgust);
            Assert.IsTrue(record.IsFear);
            Assert.IsFalse(record.IsJoy);
            Assert.IsTrue(record.IsNegative);
            Assert.IsFalse(record.IsPositive);
            Assert.IsFalse(record.IsSadness);
            Assert.IsFalse(record.IsSurprise);
            Assert.IsFalse(record.IsTrust);

            record = dictionary.FindRecord("kill");
            Assert.IsFalse(record.IsAnger);
            Assert.IsFalse(record.IsAnticipation);
            Assert.IsFalse(record.IsDisgust);
            Assert.IsTrue(record.IsFear);
            Assert.IsFalse(record.IsJoy);
            Assert.IsTrue(record.IsNegative);
            Assert.IsFalse(record.IsPositive);
            Assert.IsTrue(record.IsSadness);
            Assert.IsFalse(record.IsSurprise);
            Assert.IsFalse(record.IsTrust);

            record = dictionary.FindRecord("xxx");
            Assert.IsNull(record);
        }
    }
}
