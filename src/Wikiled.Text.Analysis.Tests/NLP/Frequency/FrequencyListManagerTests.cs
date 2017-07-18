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
            Assert.AreEqual(347, FrequencyListManager.Instance.BNC.GetIndex("mother"));
            Assert.AreEqual(900, FrequencyListManager.Instance.Internet.GetIndex("mother"));
            Assert.AreEqual(2129, FrequencyListManager.Instance.Reuters.GetIndex("mother"));
            Assert.AreEqual(241, FrequencyListManager.Instance.Subtitles.GetIndex("mother"));
        }
    }
}
