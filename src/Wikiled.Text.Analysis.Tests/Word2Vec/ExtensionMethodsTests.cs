using NUnit.Framework;
using System.IO;
using System.Linq;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(1313578, vector.Count);
        }

        [Test]
        public void GetTopWords()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            model.PopulateDictionary(GetPath("model.dic"));
            model = model.GetTopWords(2);
            ClassicAssert.AreEqual(2, model.Words);
            ClassicAssert.AreEqual("a", model.Vectors.First().Word);
            ClassicAssert.AreEqual("the", model.Vectors.Skip(1).First().Word);
                
        }

        private string GetPath(string path)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Word2Vec", "Data", path);
        }
    }
}
