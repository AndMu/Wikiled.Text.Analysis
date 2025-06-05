using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Common.Serialization;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Structure
{
    [TestFixture]
    public class SentenceItemTests
    {
        private SentenceItem item;

        [SetUp]
        public void Setup()
        {
            item = new SentenceItem("Test");
        }

        [Test]
        public void Construct()
        {
            ClassicAssert.AreEqual("Test", item.Text);
            ClassicAssert.AreEqual(0, item.Words.Count);
        }

        [Test]
        public void Add()
        {
            ClassicAssert.AreEqual(0, item.Words.Count);
            item.Add("Test");
            ClassicAssert.AreEqual(1, item.Words.Count);
            item.Add(new WordEx(new SimpleWord("Test")));
            ClassicAssert.AreEqual(2, item.Words.Count);
        }

        [Test]
        public void Clone()
        {
            item.Add("One");
            item.Add("Two");
            var word = new WordEx("T");
            word.CalculatedValue = 2;
            item.Add(word);
            var sentence = (SentenceItem)item.Clone();
            ClassicAssert.AreEqual(3, sentence.Words.Count);
            ClassicAssert.AreEqual(item.CalculateSentiment().RawRating, sentence.CalculateSentiment().RawRating);
            ClassicAssert.AreEqual("One", sentence.Words[0].Text);
            ClassicAssert.AreEqual("Two", sentence.Words[1].Text);
            ClassicAssert.AreEqual("T", sentence.Words[2].Text);
        }

        [Test]
        public void Serialize()
        {
            item.Add("Test1");
            item.Add("Test2");
            var json = JsonConvert.SerializeObject(item);
            var itemDeserialized = JsonConvert.DeserializeObject<SentenceItem>(json);
            ClassicAssert.AreEqual(2, itemDeserialized.Words.Count);
            ClassicAssert.AreEqual("Test", itemDeserialized.Text);

            XDocument document = item.XmlSerialize();
            itemDeserialized = document.XmlDeserialize<SentenceItem>();
            ClassicAssert.AreEqual(2, itemDeserialized.Words.Count);
            ClassicAssert.AreEqual("Test", itemDeserialized.Text);
        }
    }
}
