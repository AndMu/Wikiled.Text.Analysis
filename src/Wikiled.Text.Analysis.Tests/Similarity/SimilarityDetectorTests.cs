using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using NUnit.Framework;
using System;
using System.Linq;
using Wikiled.MachineLearning.Mathematics.Vectors;
using Wikiled.Text.Analysis.Similarity;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Similarity
{
    [TestFixture]
    public class SimilarityDetectorTests
    {
        private Mock<IOneHotEncoder> mockOneHotEncoder;
        private Mock<IDistance> mockDistance;

        private SimilarityDetector instance;

        [SetUp]
        public void SetUp()
        {
            mockOneHotEncoder = new Mock<IOneHotEncoder>();
            mockDistance = new Mock<IDistance>();
            instance = CreateInstance();
        }

        [Test]
        public void Similarity()
        {
            instance.Register(BagOfWords.Create("one", "two", "three"));
            instance.Register(BagOfWords.Create("one", "one", "one"));
            instance.Register(BagOfWords.Create("one", "one", "two"));

            var result = instance.FindSimilar(BagOfWords.Create("thee", "two", "three")).ToArray();
            Assert.AreEqual(3, result.Length);
        }

        [Test]
        public void Construct()
        {
            Assert.Throws<ArgumentNullException>(() => new SimilarityDetector(
                                                     null,
                                                     mockOneHotEncoder.Object,
                                                     mockDistance.Object));

            Assert.Throws<ArgumentNullException>(() => new SimilarityDetector(
                                                     new NullLogger<SimilarityDetector>(),
                                                     null,
                                                     mockDistance.Object));

            Assert.Throws<ArgumentNullException>(() => new SimilarityDetector(
                                                     new NullLogger<SimilarityDetector>(),
                                                     mockOneHotEncoder.Object,
                                                     null));

        }

        private SimilarityDetector CreateInstance()
        {
            return new SimilarityDetector(new NullLogger<SimilarityDetector>(),
                                          new OneHotEncoder(),
                                          new CosineSimilarityDistance());
        }
    }
}
