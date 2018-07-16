using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
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
            Assert.AreEqual("Test", item.Text);
            Assert.AreEqual(0, item.Words.Count);
        }

        [Test]
        public void Add()
        {
            Assert.AreEqual(0, item.Words.Count);
            item.Add("Test");
            Assert.AreEqual(1, item.Words.Count);
            item.Add(new WordEx(new SimpleWord("Test")));
            Assert.AreEqual(2, item.Words.Count);
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
            Assert.AreEqual(3, sentence.Words.Count);
            Assert.AreEqual(item.CalculateSentiment().RawRating, sentence.CalculateSentiment().RawRating);
            Assert.AreNotSame(word, sentence.Words[2]);
            Assert.AreEqual("One", sentence.Words[0].Text);
            Assert.AreEqual("Two", sentence.Words[1].Text);
            Assert.AreEqual("T", sentence.Words[2].Text);
        }

        [Test]
        public void Serialize()
        {
            item.Add("Test1");
            item.Add("Test2");
            var json = JsonConvert.SerializeObject(item);
            var itemDeserialized = JsonConvert.DeserializeObject<SentenceItem>(json);
            Assert.AreEqual(2, itemDeserialized.Words.Count);
            Assert.AreEqual("Test", itemDeserialized.Text);

            XDocument document = item.XmlSerialize();
            itemDeserialized = document.XmlDeserialize<SentenceItem>();
            Assert.AreEqual(2, itemDeserialized.Words.Count);
            Assert.AreEqual("Test", itemDeserialized.Text);
        }
    }
}
