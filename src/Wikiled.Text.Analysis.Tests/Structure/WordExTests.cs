using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
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
            ClassicAssert.AreEqual(word.UnderlyingWord.Text, deserialized.UnderlyingWord.Text);
            ClassicAssert.AreEqual(11.11, deserialized.Value);

            XDocument doc = word.XmlSerialize();
            deserialized = doc.XmlDeserialize<WordEx>();
            ClassicAssert.AreEqual(11.11, deserialized.Value);
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
            ClassicAssert.AreEqual(11.11, wordClone.Value);
            ClassicAssert.AreEqual(2, wordClone.CalculatedValue);
            ClassicAssert.AreEqual(true, wordClone.IsInvertor);
            ClassicAssert.AreEqual(1, wordClone.Id);
            ClassicAssert.AreEqual(NamedEntities.Date, wordClone.EntityType);
        }
    }
}
