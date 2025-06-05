using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Word2Vec;

namespace Wikiled.Text.Analysis.Tests.Word2Vec
{
    [TestFixture]
    public class LoadingTests
    {
        [Test]
        public void TestLoadingText()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            TestLoadedModel(model);
        }

        [Test]
        public void TestLoadingTextInAnotherCulture()
        {
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CreateSpecificCulture("fr-FR");
            var model = WordModel.Load(GetPath("model.txt"));
            TestLoadedModel(model);
        }

        [Test]
        public void TestLoadingCompressedText()
        {
            var model = WordModel.Load(GetPath("model.txt.gz"));
            TestLoadedModel(model);
        }

        [Test]
        public void TestReLoadingText()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            IWordModel wordModel;
            using (var stream = new MemoryStream())
            {
                using (var writer = new TextModelWriter(stream, true))
                {
                    writer.Write(model);
                }
                stream.Seek(0, SeekOrigin.Begin);
                var tmr = new TextModelReader(new NullLoggerFactory(), stream);
                {
                    wordModel = WordModel.Load(tmr);
                }
            }

            ClassicAssert.AreEqual(model.Words, wordModel.Words);
            ClassicAssert.AreEqual(model.Size, wordModel.Size);
        }

        [Test]
        public void TestLoadingBinary()
        {
            var model = WordModel.Load(GetPath(@"model.bin"));
            TestLoadedModel(model);
        }

        [Test]
        public void TestUnknown()
        {
            var model = WordModel.Load(GetPath(@"model.bin"));
            var sentences = new[] { new SentenceItem(), new SentenceItem() };
            sentences[0].Words.Add(new WordEx("ssd"));

            sentences[1].Words.Add(new WordEx("gfgf"));

            var paragraph = model.GetParagraphVector(sentences);
            var paragraph2 = model.GetParagraphVector(sentences.Take(1).ToArray());
            ClassicAssert.AreEqual(paragraph2, paragraph);
        }

        [Test]
        public void TestLoadingCompressedBinary()
        {
            var model = WordModel.Load(GetPath(@"model.bin.gz"));
            TestLoadedModel(model);
        }

        [Test]
        public void TestLoadingTextFileWithNoHeader()
        {
            var model = WordModel.Load(GetPath("modelWithNoHeader.txt"));
            ClassicAssert.AreEqual(2, model.Words);
        }

        [Test]
        public void TestReLoadingBinary()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            IWordModel wordModel;
            using (var memoryStream = new MemoryStream())
            {
                using (var writer = new BinaryModelWriter(memoryStream, true))
                {
                    writer.Write(model);
                }

                memoryStream.Seek(0, SeekOrigin.Begin);
                var tmr = new BinaryModelReader(new NullLoggerFactory(), memoryStream);
                wordModel = WordModel.Load(tmr);
            }

            ClassicAssert.AreEqual(model.Words, wordModel.Words);
            ClassicAssert.AreEqual(model.Size, wordModel.Size);
        }

        private static void TestLoadedModel(IWordModel model)
        {
            ClassicAssert.IsNotNull(model);
            ClassicAssert.AreEqual(4501, model.Words);
            ClassicAssert.AreEqual(100, model.Size);
            ClassicAssert.AreEqual(4501, model.Vectors.Count());
            ClassicAssert.IsTrue(model.Vectors.Any(x => x.Word == "whale"));

            var whale = model.GetByWord("whale");
            ClassicAssert.IsNotNull(whale);

            var xyz = model.GetByWord("xyz");
            ClassicAssert.IsNull(xyz);

            var results = model.Nearest(whale.Vector).Take(10).ToArray();
            ClassicAssert.AreEqual(10, results.Length);
            ClassicAssert.AreEqual("whale", results[0].Word);

            var results2 = model.Nearest("whale").Take(10).ToArray();
            ClassicAssert.AreEqual(10, results2.Length);
            ClassicAssert.AreNotEqual("whale", results2[0].Word);
            ClassicAssert.AreEqual("whale,", results2[0].Word);

            var nearest = model.NearestSingle(model.GetByWord("whale").Subtract(model.GetByWord("sea")));
            ClassicAssert.IsNotNull(nearest);

            ClassicAssert.AreNotEqual(0, model.Distance("whale", "boat"));

            var king = model.GetByWord("whale");
            var man = model.GetByWord("boat");
            var woman = model.GetByWord("sea");

            var vector = king.Subtract(man).Add(woman);
            Console.WriteLine(model.NearestSingle(vector));
            var sentences = new[] { new SentenceItem(), new SentenceItem() };
            sentences[0].Words.Add(new WordEx("whale"));
            sentences[0].Words.Add(new WordEx("boat"));

            sentences[1].Words.Add(new WordEx("whale"));
            sentences[1].Words.Add(new WordEx{ Raw = "boat" });

            var paragraph = model.GetParagraphVector(sentences);
            var paragraph2 = model.GetParagraphVector(sentences.Take(1).ToArray());
            ClassicAssert.AreEqual(paragraph2, paragraph);
        }

        [Test]
        public void TestVectorAddition()
        {
            var x = new float[] { 1, 2, 3 };
            var result = x.Add(x);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(2, result[0]);
            ClassicAssert.AreEqual(4, result[1]);
            ClassicAssert.AreEqual(6, result[2]);
        }

        [Test]
        public void TestVectorSubtraction()
        {
            var x = new float[] { 1, 2, 3 };
            var result = x.Subtract(x);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(0, result[0]);
            ClassicAssert.AreEqual(0, result[1]);
            ClassicAssert.AreEqual(0, result[2]);
        }

        [Test]
        public void TestVectorDistance()
        {
            var x = new float[] { 1, 3, 4 };
            var y = new float[] { 1, 0, 0 };
            var result = x.Distance(y);
            ClassicAssert.IsNotNull(result);
            ClassicAssert.AreEqual(5, result);
        }

        [Test]
        public void TestVectorAdditionOperator()
        {
            var x = new WordVector(0, "word1", new float[] { 1, 3, 4 });
            var y = new WordVector(1, "word2", new float[] { 1, 0, 0 });
            var z = x + y;

            ClassicAssert.IsNotNull(z);
            CollectionAssert.AreEqual(new float[] { 2, 3, 4 }, z);
        }

        [Test]
        public void TestVectorSubtractionOperator()
        {
            var x = new WordVector(0, "word1", new float[] { 1, 3, 4 });
            var y = new WordVector(1, "word2", new float[] { 1, 0, 0 });
            var z = y + x - y;

            ClassicAssert.IsNotNull(z);
            CollectionAssert.AreEqual(new float[] { 1, 3, 4 }, z);
        }
        [Test]
        public void TestVectorAverage()
        {
            var x = new WordVector(0, "word1", new float[] { 1, 3, 4 });
            var y = new WordVector(1, "word2", new float[] { 1, 0, 0 });
            var result = new[] { x, y }.Average();
            CollectionAssert.AreEqual(new [] { 1, 1.5f, 2 }, result);
        }

        private string GetPath(string path)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Word2Vec", "Data", path);
        }
    }
}
