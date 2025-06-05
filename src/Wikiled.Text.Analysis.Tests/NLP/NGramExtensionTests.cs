using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.NLP;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.NLP
{
    [TestFixture]
    public class NGramExtensionTests
    {
        [Test]
        public void GetNGram()
        {
            List<WordEx> words = new List<WordEx>();
            words.Add(new WordEx(new SimpleWord("Test")) { POS = "NN" });
            words.Add(new WordEx(new SimpleWord("Test1")) { POS = "VB" });
            words.Add(new WordEx(new SimpleWord("Test2")) { POS = "NN" });
            words.Add(new WordEx(new SimpleWord("Test3")) { POS = "VB" });
            var result = words.ToArray().GetNGram().ToArray();
            ClassicAssert.AreEqual(2, result.Length);
            ClassicAssert.AreEqual("Test Test1 Test2", result[0].WordMask);
            ClassicAssert.AreEqual("NN VB NN", result[0].PosMask);
            ClassicAssert.AreEqual("Test1 Test2 Test3", result[1].WordMask);
            ClassicAssert.AreEqual("VB NN VB", result[1].PosMask);
        }
    }
}
