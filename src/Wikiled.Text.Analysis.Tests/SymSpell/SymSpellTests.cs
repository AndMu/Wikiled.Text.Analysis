using System.Collections.Generic;
using System.Threading.Tasks;
using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.SymSpell;

namespace Wikiled.Text.Analysis.Tests.SymSpell
{
    [TestFixture]
    public class SymSpellTests
    {
        private ISymSpell instance;

        private ISymSpellCompound instanceCompound;

        private FrequencyListManager manager;

        [OneTimeSetUp]
        public async Task Setup()
        {
            manager = new FrequencyListManager();
            var factory = new SymSpellFactory(manager.Common);
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() => instance = factory.Construct()));
            tasks.Add(Task.Run(() => instanceCompound = factory.ConstructCompound()));
            await Task.WhenAll(tasks).ConfigureAwait(false);
        }

        [TestCase("goood", "good")]
        [TestCase("gooodd", "good")]
        [TestCase("PErfecttt", "perfect")]
        [TestCase("Preace", "peace")]
        public void Construct(string word, string expected)
        {
            var result = instance.Lookup(word);
            Assert.AreEqual(expected, result[0].Term);
        }

        [TestCase("whereis th elove hehad dated forImuch of thepast who couqdn'tread in sixthgrade and ins pired him",
            "where is the love he had dated for much of the past who couldn't read in sixth grade and inspired")]
        public void ConstructCompound(string word, string expected)
        {
            var result = instanceCompound.LookupCompound(word);
            Assert.AreEqual(expected, result[0].Term);
        }
    }
}
