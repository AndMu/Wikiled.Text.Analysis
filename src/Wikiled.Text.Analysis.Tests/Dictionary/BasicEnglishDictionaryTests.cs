using NUnit.Framework;
using Wikiled.Text.Analysis.Dictionary;

namespace Wikiled.Text.Analysis.Tests.Dictionary
{
    [TestFixture]
    public class BasicEnglishDictionaryTests
    {
        [Test]
        public void Test()
        {
            var data = BasicEnglishDictionary.Instance.GetWords();
            Assert.AreEqual(44323, data.Length);
            Assert.IsTrue(BasicEnglishDictionary.Instance.IsKnown("mother"));
            Assert.IsFalse(BasicEnglishDictionary.Instance.IsKnown("motherzzz"));
        }
    }
}
