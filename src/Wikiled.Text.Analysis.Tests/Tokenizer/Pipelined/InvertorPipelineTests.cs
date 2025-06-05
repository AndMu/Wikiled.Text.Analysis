using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.POS.Tags;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tests.Tokenizer.Pipelined
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
                new WordEx("Two") {IsInvertor = true},
                new WordEx("Three"),
                new WordEx("Four"),
                new WordEx("X") {POS = CoordinatingConjunction.Instance.Tag},
                new WordEx("Five")
            };

            WordEx[] result = new InvertorPipeline().Process(data).ToArray();
            ClassicAssert.AreEqual(5, result.Length);
            ClassicAssert.AreEqual("One", result[0].ItemText);
            ClassicAssert.AreEqual("Three", result[1].ItemText);
            ClassicAssert.AreEqual("not_Three", result[1].Text);
            ClassicAssert.AreEqual("Four", result[2].ItemText);
            ClassicAssert.AreEqual("not_Four", result[2].Text);
            ClassicAssert.AreEqual("X", result[3].ItemText);
            ClassicAssert.AreEqual("X", result[3].Text);
            ClassicAssert.AreEqual("Five", result[4].ItemText);
            ClassicAssert.AreEqual("Five", result[4].Text);
        }
    }
}
