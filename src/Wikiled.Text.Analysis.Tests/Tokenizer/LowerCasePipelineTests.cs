using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.Tokenizer;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class LowerCasePipelineTests
    {
        [Test]
        public void Process()
        {
            string[] data = new[] {"Test", string.Empty};
            string[] results = new LowerCasePipeline().Process(data).ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.AreEqual("test", results[0]);
        }
    }
}
