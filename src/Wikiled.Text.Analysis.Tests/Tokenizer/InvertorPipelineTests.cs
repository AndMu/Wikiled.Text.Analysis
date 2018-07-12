using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.POS.Tags;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Tokenizer;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class InvertorPipelineTests
    {
        [Test]
        public void Process()
        {
            WordEx[] data =
            {
                new WordEx("One"),
                new WordEx("Two") {IsInverted = true},
                new WordEx("Three"),
                new WordEx("Four"),
                new WordEx("X") {Type = CoordinatingConjunction.Instance.Tag},
                new WordEx("Five")
            };

            WordEx[] result = new InvertorPipeline().Process(data).ToArray();
            Assert.AreEqual(5, result.Length);
            Assert.AreEqual("One", result[0].ItemText);
            Assert.AreEqual("Three", result[1].ItemText);
            Assert.AreEqual("not_Three", result[1].Text);
            Assert.AreEqual("Four", result[2].ItemText);
            Assert.AreEqual("not_Four", result[2].Text);
            Assert.AreEqual("X", result[3].ItemText);
            Assert.AreEqual("X", result[3].Text);
            Assert.AreEqual("Five", result[4].ItemText);
            Assert.AreEqual("Five", result[4].Text);
        }
    }
}
