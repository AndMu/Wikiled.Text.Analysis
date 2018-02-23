using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Wikiled.Common.Serialization;
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
            var json = JsonConvert.SerializeObject(word);
            WordEx deserialized = JsonConvert.DeserializeObject<WordEx>(json);
            Assert.AreEqual(word.UnderlyingWord.Text, deserialized.UnderlyingWord.Text);
            Assert.AreEqual(11.11, deserialized.Value);
            Assert.AreEqual(2, deserialized.Theta);

            XDocument doc = word.XmlSerialize();
            deserialized = doc.XmlDeserialize<WordEx>();
            Assert.AreEqual(11.11, deserialized.Value);
            Assert.AreEqual(2, deserialized.Theta);
        }
    }
}
