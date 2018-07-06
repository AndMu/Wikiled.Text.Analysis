using NUnit.Framework;
using System;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Structure
{
    [TestFixture]
    public class SimpleWordTests
    {
        private SimpleWord instance;

        [SetUp]
        public void Setup()
        {
            instance = CreateSimpleWord();
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentException>(() => new SimpleWord(null));
            
        }

        private SimpleWord CreateSimpleWord()
        {
            return new SimpleWord("Word");
        }
    }
}
