using System.Xml.Linq;
using Newtonsoft.Json;
using NUnit.Framework;
using Wikiled.Core.Utility.Serialization;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Structure
{
    [TestFixture]
    public class SentenceItemTests
    {
        [Test]
        public void Construct()
        {
            SentenceItem item = new SentenceItem("Test");
            Assert.AreEqual("Test", item.Text);
            Assert.AreEqual(0, item.Words.Count);
        }

        [Test]
        public void Add()
        {
            SentenceItem item = new SentenceItem("Test");
            Assert.AreEqual(0, item.Words.Count);
            item.Add("Test");
            Assert.AreEqual(1, item.Words.Count);
            item.Add(new WordEx(new SimpleWord("Test")));
            Assert.AreEqual(2, item.Words.Count);
        }

        [Test]
        public void Serialize()
        {
            SentenceItem item = new SentenceItem("Test");
            item.Add("Test1");
            item.Add("Test2");
            var json = JsonConvert.SerializeObject(item);
            var itemDeserialized = JsonConvert.DeserializeObject<SentenceItem>(json);
            Assert.AreEqual(2, itemDeserialized.Words.Count);
            Assert.AreEqual("Test", itemDeserialized.Text);
        }
    }
}
