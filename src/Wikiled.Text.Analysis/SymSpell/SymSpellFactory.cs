using System.Collections.Generic;
using System.Linq;
using Wikiled.Common.Arguments;
using Wikiled.Text.Analysis.NLP.Frequency;

namespace Wikiled.Text.Analysis.SymSpell
{
    public class SymSpellFactory
    {
        private readonly IWordFrequencyList frequency;

        private readonly int? topWords;

        public SymSpellFactory(IWordFrequencyList frequency, int? topWords = null)
        {
            Guard.NotNull(() => frequency, frequency);
            this.frequency = frequency;
            this.topWords = topWords;
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
            return frequency.All.Where(item => !topWords.HasValue || item.Index <= topWords);
        }
    }
}
