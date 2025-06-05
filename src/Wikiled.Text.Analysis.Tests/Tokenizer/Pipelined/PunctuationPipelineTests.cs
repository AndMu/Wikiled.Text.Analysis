using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tests.Tokenizer.Pipelined
{
    [TestFixture]
    public class PunctuationPipelineTests
    {
        [Test]
        public void Process()
        {
            var result = new PunctuationPipeline().Process(new[] { "father's", "day", "(really),", "$day", "#xxx" }).ToArray();
            ClassicAssert.AreEqual(8, result.Length);
            ClassicAssert.AreEqual("father's", result[0]);
            ClassicAssert.AreEqual("day", result[1]);
            ClassicAssert.AreEqual("(", result[2]);
            ClassicAssert.AreEqual("really", result[3]);
            ClassicAssert.AreEqual(")", result[4]);
            ClassicAssert.AreEqual(",", result[5]);
            ClassicAssert.AreEqual("$day", result[6]);
            ClassicAssert.AreEqual("#xxx", result[7]);
        }
    }
}
