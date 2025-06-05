using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.Words;

namespace Wikiled.Text.Analysis.Tests.POS
{
    [TestFixture]
    public class NaivePOSTaggerTests
    {
        private NaivePOSTagger instance;

        [OneTimeSetUp]
        public void Setup()
        {
            instance = new NaivePOSTagger(new BNCList(), WordTypeResolver.Instance);
        }

        [Test]
        public void GetTag()
        {
            var value = instance.GetTag(",");
            ClassicAssert.AreEqual(POSTags.Instance.Comma, value);
            value = instance.GetTag("!");
            ClassicAssert.AreEqual(POSTags.Instance.SYM, value);
            value = instance.GetTag("axe");
            ClassicAssert.AreEqual(POSTags.Instance.NN, value);
            value = instance.GetTag("abject");
            ClassicAssert.AreEqual(POSTags.Instance.JJ, value);
            value = instance.GetTag("utterly");
            ClassicAssert.AreEqual(POSTags.Instance.RB, value);
            value = instance.GetTag("around");
            ClassicAssert.AreEqual(POSTags.Instance.RP, value);
            value = instance.GetTag("xxxx1");
            ClassicAssert.AreEqual(POSTags.Instance.UnknownWord, value);
            value = instance.GetTag("a");
            ClassicAssert.AreEqual(POSTags.Instance.RP, value);
            value = instance.GetTag("each");
            ClassicAssert.AreEqual(POSTags.Instance.PRP, value);
            value = instance.GetTag("run");
            ClassicAssert.AreEqual(POSTags.Instance.VB, value);
        }
    }
}
