using System.Linq;
using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Wikiled.Core.Utility.Serialization;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Structure
{
    [TestFixture]
    public class DocumentTests
    {
        [Test]
        public void Construct()
        {
            Document document = new Document("Test");
            Assert.AreEqual("Test", document.Text);
            Assert.AreEqual(0, document.Sentences.Count);
        }

        [Test]
        public void Add()
        {
            Document document = new Document("Test");
            Assert.AreEqual(0, document.Sentences.Count);
            document.Add(new SentenceItem());
            Assert.AreEqual(1, document.Sentences.Count);
        }

        [Test]
        public void Words()
        {
            Document document = new Document("Test");
            Assert.AreEqual(0, document.TotalWords);
            Assert.AreEqual(0, document.Words.Count());
            document.Add(new SentenceItem());
            Assert.AreEqual(0, document.TotalWords);
            Assert.AreEqual(0, document.Words.Count());
            Assert.AreEqual(1, document.Sentences.Count);
            document.Add(new SentenceItem());
            Assert.AreEqual(2, document.Sentences.Count);
            document.Sentences[0].Add(new WordEx(new SimpleWord("Test")));
            document.Sentences[1].Add(new WordEx(new SimpleWord("Test")));
            Assert.AreEqual(2, document.TotalWords);
            Assert.AreEqual(2, document.Words.Count());
        }

        [Test]
        public void Serialize()
        {
            Document document = new Document("Test");
            document.Add(new SentenceItem());
            document.Sentences[0].Add("Test Word");
            document.Add(new SentenceItem());
            var json = JsonConvert.SerializeObject(document);
            Document documentDeserialized = JsonConvert.DeserializeObject<Document>(json);
            Assert.AreEqual(2, documentDeserialized.Sentences.Count);
            Assert.AreEqual("Test", documentDeserialized.Text);

            XDocument xDocument = document.XmlSerialize();
            documentDeserialized = xDocument.XmlDeserialize<Document>();
            Assert.AreEqual(2, documentDeserialized.Sentences.Count);
            Assert.AreEqual("Test", documentDeserialized.Text);
        }
    }
}
