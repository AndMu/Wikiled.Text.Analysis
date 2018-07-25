using System;
using System.Globalization;
using System.IO;
using System.Linq;
using NUnit.Framework;
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
            WordModel m2;
            using (var s = new MemoryStream())
            {
                using (var writer = new TextModelWriter(s, true))
                {
                    writer.Write(model);
                }
                s.Seek(0, SeekOrigin.Begin);
                var tmr = new TextModelReader(s);
                {
                    m2 = WordModel.Load(tmr);
                }
            }

            Assert.AreEqual(model.Words, m2.Words);
            Assert.AreEqual(model.Size, m2.Size);
        }

        [Test]
        public void TestLoadingBinary()
        {
            var model = WordModel.Load(GetPath(@"model.bin"));
            TestLoadedModel(model);
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
            Assert.AreEqual(2, model.Words);
        }

        [Test]
        public void TestReLoadingBinary()
        {
            var model = WordModel.Load(GetPath("model.txt"));
            WordModel m2;
            using (var s = new MemoryStream())
            {
                using (var writer = new BinaryModelWriter(s, true))
                {
                    writer.Write(model);
                }
                s.Seek(0, SeekOrigin.Begin);
                var tmr = new BinaryModelReader(s);
                m2 = WordModel.Load(tmr);
            }

            Assert.AreEqual(model.Words, m2.Words);
            Assert.AreEqual(model.Size, m2.Size);
        }

        private static void TestLoadedModel(WordModel model)
        {
            Assert.IsNotNull(model);
            Assert.AreEqual(4501, model.Words);
            Assert.AreEqual(100, model.Size);
            Assert.AreEqual(4501, model.Vectors.Count());
            Assert.IsTrue(model.Vectors.Any(x => x.Word == "whale"));


            var whale = model.GetByWord("whale");
            Assert.IsNotNull(whale);

            var xyz = model.GetByWord("xyz");
            Assert.IsNull(xyz);

            var results = model.Nearest(whale.Vector).Take(10).ToArray();
            Assert.AreEqual(10, results.Length);
            Assert.AreEqual("whale", results[0].Word);

            var results2 = model.Nearest("whale").Take(10).ToArray();
            Assert.AreEqual(10, results2.Length);
            Assert.AreNotEqual("whale", results2[0].Word);
            Assert.AreEqual("whale,", results2[0].Word);

            var nearest = model.NearestSingle(model.GetByWord("whale").Subtract(model.GetByWord("sea")));
            Assert.IsNotNull(nearest);

            Assert.AreNotEqual(0, model.Distance("whale", "boat"));

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
            Assert.AreEqual(paragraph2, paragraph);
        }

        [Test]
        public void TestVectorAddition()
        {
            var x = new float[] { 1, 2, 3 };
            var result = x.Add(x);
            Assert.IsNotNull(result);
            Assert.AreEqual(2, result[0]);
            Assert.AreEqual(4, result[1]);
            Assert.AreEqual(6, result[2]);
        }

        [Test]
        public void TestVectorSubtraction()
        {
            var x = new float[] { 1, 2, 3 };
            var result = x.Subtract(x);
            Assert.IsNotNull(result);
            Assert.AreEqual(0, result[0]);
            Assert.AreEqual(0, result[1]);
            Assert.AreEqual(0, result[2]);
        }

        [Test]
        public void TestVectorDistance()
        {
            var x = new float[] { 1, 3, 4 };
            var y = new float[] { 1, 0, 0 };
            var result = x.Distance(y);
            Assert.IsNotNull(result);
            Assert.AreEqual(5, result);
        }

        [Test]
        public void TestVectorAdditionOperator()
        {
            var x = new WordVector("word1", new float[] { 1, 3, 4 });
            var y = new WordVector("word2", new float[] { 1, 0, 0 });
            var z = x + y;

            Assert.IsNotNull(z);
            CollectionAssert.AreEqual(new float[] { 2, 3, 4 }, z);
        }

        [Test]
        public void TestVectorSubtractionOperator()
        {
            var x = new WordVector("word1", new float[] { 1, 3, 4 });
            var y = new WordVector("word2", new float[] { 1, 0, 0 });
            var z = y + x - y;

            Assert.IsNotNull(z);
            CollectionAssert.AreEqual(new float[] { 1, 3, 4 }, z);
        }
        [Test]
        public void TestVectorAverage()
        {
            var x = new WordVector("word1", new float[] { 1, 3, 4 });
            var y = new WordVector("word2", new float[] { 1, 0, 0 });
            var result = new[] { x, y }.Average();
            CollectionAssert.AreEqual(new [] { 1, 1.5f, 2 }, result);
        }

        private string GetPath(string path)
        {
            return Path.Combine(TestContext.CurrentContext.TestDirectory, "Word2Vec", "Data", path);
        }
    }
}
