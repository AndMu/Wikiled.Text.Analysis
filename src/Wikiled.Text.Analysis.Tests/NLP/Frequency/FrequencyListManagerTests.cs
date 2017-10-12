using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;

namespace Wikiled.Text.Analysis.Tests.NLP.Frequency
{
    [TestFixture]
    public class FrequencyListManagerTests
    {
        [Test]
        public void Test()
        {
            var instance = new FrequencyListManager();
            Assert.AreEqual(347, instance.BNC.GetIndex("mother").Index);
            Assert.AreEqual(900, instance.Internet.GetIndex("mother").Index);
            Assert.AreEqual(2129, instance.Reuters.GetIndex("mother").Index);
            Assert.AreEqual(241, instance.Subtitles.GetIndex("mother").Index);
        }
    }
}
