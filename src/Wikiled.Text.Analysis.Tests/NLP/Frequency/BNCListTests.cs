using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Tests.NLP.Frequency
{
    [TestFixture]
    public class BNCListTests
    {
        private BNCList instance;

        [SetUp]
        public void Setup()
        {
            instance = new BNCList();
        }

        [Test]
        public void GetIndex()
        {
            Assert.AreEqual(10, instance.GetIndex("was"));
            Assert.AreEqual(22, instance.GetIndex("have"));
            Assert.AreEqual(32, instance.GetIndex("which"));
        }


        [Test]
        public void GetPOS()
        {
            Assert.AreEqual(POSTags.Instance.VBD, instance.GetPOS("was"));
            Assert.AreEqual(POSTags.Instance.VBP, instance.GetPOS("have"));
            Assert.AreEqual(POSTags.Instance.WDT, instance.GetPOS("which"));
        }
    }
}
