using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Twitter;

namespace Wikiled.Text.Analysis.Tests.Twitter
{
    [TestFixture]
    public class StringExtensionsTest
    {
        /// <summary>
        /// 
        /// </summary>
        [Test]
        public void SliceTest()
        {
            string str;
            str = "Hello world!";
            ClassicAssert.AreEqual("Hello world!", str.Slice(0));
            ClassicAssert.AreEqual("lo world!", str.Slice(3));
            ClassicAssert.AreEqual("lo wo", str.Slice(3, 8));
            ClassicAssert.AreEqual("H", str.Slice(0, 1));
            ClassicAssert.AreEqual("!", str.Slice(-1));
            ClassicAssert.AreEqual("lo world", str.Slice(3, -1));
            ClassicAssert.AreEqual("", str.Slice(-1, -1));
        }
    }
}