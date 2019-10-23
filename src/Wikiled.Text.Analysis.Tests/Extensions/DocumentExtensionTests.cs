using System.Buffers;
using NUnit.Framework;
using Wikiled.Text.Analysis.Extensions;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Tests.Extensions
{
    [TestFixture]
    public class DocumentExtensionTests
    {
        private LightDocument document;

        [SetUp]
        public void Setup()
        {
            document = new LightDocument();
        }

        [TestCase("One", "Text", "Text:Text__End__Text__Len__4")]
        public void GetTextId(string id, string text, string expected)
        {
            document.Id = id;
            document.Text = text;
            var result = document.GetTextId();
            Assert.AreEqual(expected, result);
        }

        [TestCase("One", "Text", "Document:One:Text__End__Text__Len__4")]
        public void GetId(string id, string text, string expected)
        {
            document.Id = id;
            document.Text = text;
            var result = document.GetId();
            Assert.AreEqual(expected, result);
        }

        [Test]
        public void GetLight()
        {
            var fullDocument = new Document("Test");
            fullDocument.Add(new SentenceItem());
            fullDocument.Sentences[0].Add("Test Word");
            fullDocument.Add(new SentenceItem());
            var result = fullDocument.GetLight();
            Assert.AreEqual("Test", result.Text);
            Assert.AreEqual(2, result.Sentences.Length);
            Assert.AreEqual(1, result.Sentences[0].Words.Length);
            Assert.AreEqual("Test Word", result.Sentences[0].Words[0].Text);
        }
    }
}
