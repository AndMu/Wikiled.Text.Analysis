using System.Runtime.Caching;
using NUnit.Framework;
using Wikiled.Text.Analysis.Dictionary;
using Wikiled.Text.Analysis.NLP;

namespace Wikiled.Text.Analysis.Tests.NLP
{
    [TestFixture]
    public class RawWordExtractorTests
    {
        private RawWordExtractor instance;

        [SetUp]
        public void Setup()
        {
            instance = new RawWordExtractor(BasicEnglishDictionary.Instance, new MemoryCache("Test"));
        }

        [TestCase("program's", "program")]
        [TestCase("ringtones", "ringtone")]

        [TestCase("smallest", "small")]
        [TestCase("best", "best")]
        [TestCase("prettier", "pretty")]
        [TestCase("noblest", "noble")]
        [TestCase("browser", "browser")]

        [TestCase("browsers", "browser")]
        [TestCase("browsing", "browse")]
        [TestCase("uses", "us")]
        [TestCase("using", "use")]
        [TestCase("dyeing", "dye")]
        [TestCase("miss", "miss")]
        [TestCase("seeing", "see")]
        [TestCase("frustrating", "frustrate")]
        [TestCase("fanned", "fan")]
        [TestCase("liked", "like")]
        [TestCase("missed", "miss")]
        [TestCase("helped", "help")]
        [TestCase("button", "button")]
        [TestCase("Itunes", "itune")]
        [TestCase("years", "year")]
        [TestCase("productive", "productive")]
        [TestCase("potatoes", "potato")]
        [TestCase("Cars", "car")]
        [TestCase("was", "was")]
        [TestCase("wasnt", "wasnt")]
        [TestCase("cannt", "cannt")]
        [TestCase("wouldnt", "wouldnt")]
        [TestCase("wouldn't", "wouldn't")]
        [TestCase("anti-virus", "anti-virus")]
        public void GetSpecialSymbols(string word, string extected)
        {
            var result = instance.GetWord(word);
            Assert.AreEqual(extected, result);
        }
    }
}
