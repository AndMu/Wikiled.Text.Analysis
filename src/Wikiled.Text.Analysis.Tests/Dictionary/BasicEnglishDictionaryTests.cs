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
            var instance = new BasicEnglishDictionary();
            var data = instance.GetWords();
            Assert.AreEqual(44323, data.Length);
            Assert.IsTrue(instance.IsKnown("mother"));
            Assert.IsFalse(instance.IsKnown("motherzzz"));
        }
    }
}
