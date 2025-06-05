using System.Linq;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Tokenizer;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tests.Tokenizer
{
    [TestFixture]
    public class WordTokenizerTests
    {
        [Test]
        public void GetWords()
        {
            WordTokenizer tokenizer = new WordTokenizer(
                "Test", 
                new SimpleWordItemFactory(Global.PosTagger, Global.Raw), 
                NullSimpleWordPipeline.Instance, 
                NullWordItemPipeline.Instance, 
                new[] { "test", "one" });
            var result = tokenizer.GetWords().ToArray();
            ClassicAssert.AreEqual("Test", tokenizer.SentenceText);
            ClassicAssert.AreEqual(2, result.Length);
            ClassicAssert.AreEqual("test", result[0]);
            ClassicAssert.AreEqual("one", result[1]);
        }

        [Test]
        public void GetWordsWithFilterOut()
        {
            WordTokenizer tokenizer = new WordTokenizer(
                "Test",
                new SimpleWordItemFactory(Global.PosTagger, Global.Raw), 
                NullSimpleWordPipeline.Instance, 
                NullWordItemPipeline.Instance, 
                new[] { "", "one" });
            var result = tokenizer.GetWords().ToArray();
            ClassicAssert.AreEqual(1, result.Length);
            ClassicAssert.AreEqual("one", result[0]);
        }
    }
}
