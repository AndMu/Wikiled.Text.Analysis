using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(347, instance.BNC.GetIndex("mother").Index);
            ClassicAssert.AreEqual(900, instance.Internet.GetIndex("mother").Index);
            ClassicAssert.AreEqual(2129, instance.Reuters.GetIndex("mother").Index);
            ClassicAssert.AreEqual(241, instance.Subtitles.GetIndex("mother").Index);
        }
    }
}
