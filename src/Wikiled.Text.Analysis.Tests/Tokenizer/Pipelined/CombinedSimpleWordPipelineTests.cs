using System.Linq;
using NUnit.Framework;
using Wikiled.Text.Analysis.Tokenizer;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class CombinedSimpleWordPipelineTests
    {
        [Test]
        public void Create()
        {
            CombinedPipeline<string> combined = new CombinedPipeline<string>(new LowerCasePipeline());
            Assert.AreEqual(1, combined.Pipelines.Count);
            combined = new CombinedPipeline<string>();
            Assert.AreEqual(0, combined.Pipelines.Count);
        }

        [Test]
        public void Process()
        {
            string[] data = { "Test", string.Empty };
            CombinedPipeline<string> combined = new CombinedPipeline<string>(new LowerCasePipeline());
            string[] results = combined.Process(data).ToArray();
            Assert.AreEqual(1, results.Length);
            Assert.AreEqual("test", results[0]);
        }
    }
}
