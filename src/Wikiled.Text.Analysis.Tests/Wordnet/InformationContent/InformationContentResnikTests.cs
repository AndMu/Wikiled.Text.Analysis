using System;
using System.IO;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.WordNet.Engine;
using Wikiled.Text.Analysis.WordNet.InformationContent;
using ConfigurationManager = System.Configuration.ConfigurationManager;

namespace Wikiled.Text.Analysis.Tests.Wordnet.InformationContent
{
    [TestFixture]
    public class InformationContentResnikTests
    {
        private WordNetEngine engine;

        private JcnMeasure instance;

        private InformationContentResnik resnik;

        [SetUp]
        public void Setup()
        {
            instance = new JcnMeasure(new MemoryCache(new MemoryCacheOptions()), resnik, engine);
        }

        [OneTimeSetUp]
        public void SetupGlobal()
        {
            var path = ConfigurationManager.AppSettings["resources"];
            resnik = InformationContentResnik.Load(Path.Combine(TestContext.CurrentContext.TestDirectory, path, @"WordNet-InfoContent-3.0\ic-brown-resnik-add1.dat"));
            engine = new WordNetEngine(Path.Combine(TestContext.CurrentContext.TestDirectory, path, @"Wordnet 3.0"));
        }

        [Test]
        public void GetIC()
        {
            var value = resnik.GetIC(new SynSet(WordType.Noun, 1740));
            Assert.AreEqual(0, value);
            value = resnik.GetIC(new SynSet(WordType.Noun, 2684));
            Assert.AreEqual(0.51, Math.Round(value, 2));
        }

        [Test]
        public void GetSynSet()
        {
            var value = resnik.GetFrequency(new SynSet(WordType.Noun, 1740));
            Assert.AreEqual(619463.364700726, value);
            value = resnik.GetFrequency(new SynSet(WordType.Noun, 2684));
            Assert.AreEqual(189793.245018072, value);
        }

        [Test]
        public void JcnMeasure()
        {
            var result = instance.Measure("car", "automobile");
            Assert.AreEqual(1, result);
        }

        [Test]
        public void JcnMeasureFork()
        {
            var result = instance.Measure("car", "fork");
            Assert.AreEqual(0.19, Math.Round(result, 2));
        }

        [Test]
        public void JcnMeasureForkSynsets()
        {
            var forkSyn = engine.GetSynSets("fork", WordType.Noun);
            var carSyn = engine.GetSynSets("car", WordType.Noun);
            var result = instance.Measure(forkSyn[4], carSyn[4]);
            Assert.AreEqual(0.17, Math.Round(result, 2));
        }

        [Test]
        public void JcnSimilar()
        {
            var result = instance.Measure("wheel", "circle");
            Assert.AreEqual(0.36, Math.Round(result, 2));
        }
    }
}
