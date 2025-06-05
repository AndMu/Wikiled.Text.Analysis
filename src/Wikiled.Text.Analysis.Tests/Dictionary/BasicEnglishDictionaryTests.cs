using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(44323, data.Length);
            ClassicAssert.IsTrue(instance.IsKnown("mother"));
            ClassicAssert.IsFalse(instance.IsKnown("motherzzz"));
        }
    }
}
