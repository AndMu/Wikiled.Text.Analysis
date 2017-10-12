using System.Collections.Generic;
using System.Linq;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Text.Analysis.NLP.Frequency;

namespace Wikiled.Text.Analysis.SymSpell
{
    public class SymSpellFactory
    {
        private readonly IWordFrequencyList frequency;

        private readonly int minFrequency;

        public SymSpellFactory(IWordFrequencyList frequency, int minFrequency)
        {
            Guard.NotNull(() => frequency, frequency);
            this.frequency = frequency;
            this.minFrequency = minFrequency;
        }

        public ISymSpell Construct()
        {
            SymSpellManager instance = new SymSpellManager();
            foreach (var information in GetItems())
            {
                instance.AddRecord(information.Word, (long)information.Frequency);
            }

            return instance;
        }

        public ISymSpellCompound ConstructCompound()
        {
            SymSpellCompound instance = new SymSpellCompound();
            foreach (var information in GetItems())
            {
                instance.CreateDictionaryEntry(information.Word, (long)information.Frequency);
            }

            return instance;
        }

        private IEnumerable<FrequencyInformation> GetItems()
        {
            return frequency.All.Where(item => item.Frequency >= minFrequency);
        }
    }
}
