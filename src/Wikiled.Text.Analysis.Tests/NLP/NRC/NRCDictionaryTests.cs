using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(0, vector.Anger);
            ClassicAssert.AreEqual(0, vector.Anticipation);
            ClassicAssert.AreEqual(0, vector.Disgust);
            ClassicAssert.AreEqual(1, vector.Fear);
            ClassicAssert.AreEqual(0, vector.Joy);
            ClassicAssert.AreEqual(0, vector.Trust);
            ClassicAssert.AreEqual(1, vector.Sadness);
            ClassicAssert.AreEqual(0, vector.Surprise);
            ClassicAssert.AreEqual(1, vector.Total);
            ClassicAssert.AreEqual(2, vector.TotalSum);

            vector = dictionary.Extract(new[] { new WordEx(("love")) });
            ClassicAssert.AreEqual(0, vector.Anger);
            ClassicAssert.AreEqual(0, vector.Anticipation);
            ClassicAssert.AreEqual(0, vector.Disgust);
            ClassicAssert.AreEqual(0, vector.Fear);
            ClassicAssert.AreEqual(1, vector.Joy);
            ClassicAssert.AreEqual(0, vector.Sadness);
            ClassicAssert.AreEqual(0, vector.Surprise);
            ClassicAssert.AreEqual(0, vector.Trust);
            ClassicAssert.AreEqual(1, vector.Total);
            ClassicAssert.AreEqual(1, vector.TotalSum);
        }

        [Test]
        public void FindRecord()
        {
            var record = dictionary.FindRecord("smut");
            ClassicAssert.IsFalse(record.IsAnger);
            ClassicAssert.IsFalse(record.IsAnticipation);
            ClassicAssert.IsTrue(record.IsDisgust);
            ClassicAssert.IsTrue(record.IsFear);
            ClassicAssert.IsFalse(record.IsJoy);
            ClassicAssert.IsTrue(record.IsNegative);
            ClassicAssert.IsFalse(record.IsPositive);
            ClassicAssert.IsFalse(record.IsSadness);
            ClassicAssert.IsFalse(record.IsSurprise);
            ClassicAssert.IsFalse(record.IsTrust);

            record = dictionary.FindRecord("kill");
            ClassicAssert.IsFalse(record.IsAnger);
            ClassicAssert.IsFalse(record.IsAnticipation);
            ClassicAssert.IsFalse(record.IsDisgust);
            ClassicAssert.IsTrue(record.IsFear);
            ClassicAssert.IsFalse(record.IsJoy);
            ClassicAssert.IsTrue(record.IsNegative);
            ClassicAssert.IsFalse(record.IsPositive);
            ClassicAssert.IsTrue(record.IsSadness);
            ClassicAssert.IsFalse(record.IsSurprise);
            ClassicAssert.IsFalse(record.IsTrust);

            record = dictionary.FindRecord("xxx");
            ClassicAssert.IsNull(record);
        }
    }
}
