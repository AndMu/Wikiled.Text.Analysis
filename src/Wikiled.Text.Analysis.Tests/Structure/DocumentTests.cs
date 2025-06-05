using Newtonsoft.Json;
using NUnit.Framework;
using System.Linq;
using NUnit.Framework.Legacy;
using Wikiled.Common.Serialization;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Structure
{
    [TestFixture]
    public class DocumentTests
    {
        [Test]
        public void Construct()
        {
            var document = new Document("Test");
            ClassicAssert.AreEqual("Test", document.Text);
            ClassicAssert.AreEqual(0, document.Sentences.Count);
        }

        [Test]
        public void Add()
        {
            var document = new Document("Test");
            ClassicAssert.AreEqual(0, document.Sentences.Count);
            document.Add(new SentenceItem(), false);
            ClassicAssert.AreEqual(1, document.Sentences.Count);
        }

        [Test]
        public void Words()
        {
            var document = new Document("Test");
            ClassicAssert.AreEqual(0, document.TotalWords);
            ClassicAssert.AreEqual(0, document.Words.Count());
            document.Add(new SentenceItem(), false);
            ClassicAssert.AreEqual(0, document.TotalWords);
            ClassicAssert.AreEqual(0, document.Words.Count());
            ClassicAssert.AreEqual(1, document.Sentences.Count);
            document.Add(new SentenceItem(), false);
            ClassicAssert.AreEqual(2, document.Sentences.Count);
            document.Sentences[0].Add(new WordEx(new SimpleWord("Test")));
            document.Sentences[1].Add(new WordEx(new SimpleWord("Test")));
            ClassicAssert.AreEqual(2, document.TotalWords);
            ClassicAssert.AreEqual(2, document.Words.Count());
        }

        [Test]
        public void Serialize()
        {
            var document = new Document("Test");
            document.Add(new SentenceItem(), false);
            document.Sentences[0].Add("Test Word");
            document.Add(new SentenceItem(), false);
            var json = JsonConvert.SerializeObject(document);
            var documentDeserialized = JsonConvert.DeserializeObject<Document>(json);
            ClassicAssert.AreEqual(2, documentDeserialized.Sentences.Count);
            ClassicAssert.AreEqual("Test", documentDeserialized.Text);

            var xDocument = document.XmlSerialize();
            documentDeserialized = xDocument.XmlDeserialize<Document>();
            ClassicAssert.AreEqual(2, documentDeserialized.Sentences.Count);
            ClassicAssert.AreEqual("Test", documentDeserialized.Text);
        }
    }
}
