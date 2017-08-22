using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.NRC;

namespace Wikiled.Text.Analysis.Tests.NLP.NRC
{
    [TestFixture]
    public class NRCDictionaryTests
    {
        private readonly NRCDictionary dictionary;

        public NRCDictionaryTests()
        {
            dictionary = new NRCDictionary();
        }
        
        [Test]
        public void Test()
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
