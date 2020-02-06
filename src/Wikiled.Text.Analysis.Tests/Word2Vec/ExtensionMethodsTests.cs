using NUnit.Framework;
using System.IO;
using System.Linq;
using Wikiled.Text.Analysis.Word2Vec;

namespace Wikiled.Text.Analysis.Tests.Word2Vec
{
    [TestFixture]
    public class ExtensionMethodsTests
    {
        [Test]
        public void PopulateDictionary()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            model.PopulateDictionary(GetPath("model.dic"));
            var vector = model.Find("the");
            Assert.AreEqual(1313578, vector.Count);
        }

        [Test]
        public void GetTopWords()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            model.PopulateDictionary(GetPath("model.dic"));
            model = model.GetTopWords(2);
            Assert.AreEqual(2, model.Words);
            Assert.AreEqual("a", model.Vectors.First().Word);
            Assert.AreEqual("the", model.Vectors.Skip(1).First().Word);
                
        }

        private string GetPath(string path)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Word2Vec", "Data", path);
        }
    }
}
