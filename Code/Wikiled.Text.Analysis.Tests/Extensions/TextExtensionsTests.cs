using NUnit.Framework;
using Wikiled.Text.Analysis.Extensions;

namespace Wikiled.Text.Analysis.Tests.Extensions
{
    [TestFixture]
    public class TextExtensionsTests
    {
        [TestCase('a', true)]
        [TestCase('b', false)]
        [TestCase('A', true)]
        [TestCase('U', true)]
        public void Construct(char letter, bool expected)
        {
            var result = TextExtensions.IsVowel(letter);
            Assert.AreEqual(expected, result);
        }

    }
}
