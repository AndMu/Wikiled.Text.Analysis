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
            var json = JsonConvert.SerializeObject(word);
            WordEx deserialized = JsonConvert.DeserializeObject<WordEx>(json);
            Assert.AreEqual(word.UnderlyingWord.Text, deserialized.UnderlyingWord.Text);
            Assert.AreEqual(11.11, deserialized.Value);

            XDocument doc = word.XmlSerialize();
            deserialized = doc.XmlDeserialize<WordEx>();
            Assert.AreEqual(11.11, deserialized.Value);
        }

        [Test]
        public void Clone()
        {
            WordEx word = new WordEx("Test");
            word.Value = 11.11;
            word.CalculatedValue = 2;
            word.Id = 1;
            word.IsInvertor = true;
            word.EntityType = NamedEntities.Date;

            var wordClone = (WordEx)word.Clone();
            Assert.AreEqual(11.11, wordClone.Value);
            Assert.AreEqual(2, wordClone.CalculatedValue);
            Assert.AreEqual(true, wordClone.IsInvertor);
            Assert.AreEqual(1, wordClone.Id);
            Assert.AreEqual(NamedEntities.Date, wordClone.EntityType);
        }
    }
}
