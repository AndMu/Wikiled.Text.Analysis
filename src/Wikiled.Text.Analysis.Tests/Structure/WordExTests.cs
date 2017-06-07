using System.Xml.Linq;
using NUnit.Framework;
using Wikiled.Core.Utility.Serialization;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Structure
{
    [TestFixture]
    public class WordExTests
    {
        [Test]
        public void Serialize()
        {
            WordEx word = new WordEx(new SimpleWord("Test"));
            word.Value = 11.11;
            word.Theta = 2;
            XDocument doc = word.XmlSerialize();
            WordEx deserialized = doc.XmlDeserialize<WordEx>();
            Assert.AreEqual(word.UnderlyingWord.Text, deserialized.UnderlyingWord.Text);
            Assert.AreEqual(11.11, deserialized.Value);
            Assert.AreEqual(2, deserialized.Theta);
        }
    }
}
