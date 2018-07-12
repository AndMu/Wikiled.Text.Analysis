using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
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
            words.Add(new WordEx(new SimpleWord("Test")) { Type = "NN" });
            words.Add(new WordEx(new SimpleWord("Test1")) { Type = "VB" });
            words.Add(new WordEx(new SimpleWord("Test2")) { Type = "NN" });
            words.Add(new WordEx(new SimpleWord("Test3")) { Type = "VB" });
            var result = words.ToArray().GetNGram().ToArray();
            Assert.AreEqual(2, result.Length);
            Assert.AreEqual("Test Test1 Test2", result[0].WordMask);
            Assert.AreEqual("NN VB NN", result[0].PosMask);
            Assert.AreEqual("Test1 Test2 Test3", result[1].WordMask);
            Assert.AreEqual("VB NN VB", result[1].PosMask);
        }
    }
}
