using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(10, instance.GetIndex("was").Index);
            ClassicAssert.AreEqual(22, instance.GetIndex("have").Index);
            ClassicAssert.AreEqual(32, instance.GetIndex("which").Index);
        }


        [Test]
        public void GetPOS()
        {
            ClassicAssert.AreEqual(POSTags.Instance.VBD, instance.GetPOS("was"));
            ClassicAssert.AreEqual(POSTags.Instance.VBP, instance.GetPOS("have"));
            ClassicAssert.AreEqual(POSTags.Instance.WDT, instance.GetPOS("which"));
        }
    }
}
