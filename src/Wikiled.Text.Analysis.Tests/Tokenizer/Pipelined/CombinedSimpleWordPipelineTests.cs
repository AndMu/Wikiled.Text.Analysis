using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Tokenizer;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tests.Tokenizer.Pipelined
{
    [TestFixture]
    public class CombinedSimpleWordPipelineTests
    {
        [Test]
        public void Create()
        {
            CombinedPipeline<string> combined = new CombinedPipeline<string>(new LowerCasePipeline());
            ClassicAssert.AreEqual(1, combined.Pipelines.Count);
            combined = new CombinedPipeline<string>();
            ClassicAssert.AreEqual(0, combined.Pipelines.Count);
        }

        [Test]
        public void Process()
        {
            string[] data = { "Test", string.Empty };
            CombinedPipeline<string> combined = new CombinedPipeline<string>(new LowerCasePipeline());
            string[] results = combined.Process(data).ToArray();
            ClassicAssert.AreEqual(1, results.Length);
            ClassicAssert.AreEqual("test", results[0]);
        }
    }
}
