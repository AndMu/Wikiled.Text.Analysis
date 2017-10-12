using NUnit.Framework;
using Wikiled.Text.Analysis.NLP.Frequency;
using Wikiled.Text.Analysis.SymSpell;

namespace Wikiled.Text.Analysis.Tests.SymSpell
{
    [TestFixture]
    public class SymSpellTests
    {
        private ISymSpell instance;

        [OneTimeSetUp]
        public void Setup()
        {
            var factory = new SymSpellFactory(new FrequencyListManager().BNC);
            instance = factory.Construct();
        }

        [TestCase("goood", "good")]
        [TestCase("gooodd", "good")]
        [TestCase("PErfectttt", "perfect")]
        public void Construct(string word, string expected)
        {
            var result = instance.Lookup(word, "English", 4);
        }
    }
}
