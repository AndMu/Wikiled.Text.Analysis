using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Tokenizer;

namespace Wikiled.Text.Analysis.Tests.Tokenizer.Pipelined
{
    [TestFixture]
    public class LowerCasePipelineTests
    {
        [Test]
        public void Process()
        {
            string[] data = new[] {"Test", string.Empty};
            string[] results = new LowerCasePipeline().Process(data).ToArray();
            ClassicAssert.AreEqual(1, results.Length);
            ClassicAssert.AreEqual("test", results[0]);
        }
    }
}
