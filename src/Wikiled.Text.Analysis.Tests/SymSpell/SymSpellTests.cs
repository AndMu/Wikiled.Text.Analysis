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
            var factory = new SymSpellFactory(manager.BNC, 5000);
            List<Task> tasks = new List<Task>();
            tasks.Add(Task.Run(() => instance = factory.Construct()));
            tasks.Add(Task.Run(() => instanceCompound = factory.ConstructCompound()));
            await Task.WhenAll(tasks);
        }

        [TestCase("goood", "good")]
        [TestCase("gooodd", "good")]
        [TestCase("PErfecttt", "perfect")]
        [TestCase("Preace", "peace")]
        public void Construct(string word, string expected)
        {
            var result = instance.Lookup(word, SuggestionType.All);
            Assert.AreEqual(expected, result[0].Term);
        }

        [TestCase("whereis th elove hehad dated forImuch of thepast who couqdn'tread in sixthgrade and ins pired him", "where is the love head date for much of the past who could read in six trade and in red him")]
        public void ConstructCompound(string word, string expected)
        {
            var result = instanceCompound.LookupCompound(word, 2);
            Assert.AreEqual(expected, result[0].Term);
        }
    }
}
