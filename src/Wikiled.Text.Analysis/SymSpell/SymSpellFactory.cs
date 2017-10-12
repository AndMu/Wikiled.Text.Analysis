using Wikiled.Core.Utility.Arguments;
using Wikiled.Text.Analysis.NLP.Frequency;

namespace Wikiled.Text.Analysis.SymSpell
{
    public class SymSpellFactory
    {
        private readonly IWordFrequencyList frequency;

        public SymSpellFactory(IWordFrequencyList frequency)
        {
            Guard.NotNull(() => frequency, frequency);
            this.frequency = frequency;
        }

        public ISymSpell Construct()
        {
            SymSpellManager instance = new SymSpellManager();
            foreach (var information in frequency.All)
            {
                instance.AddRecord(information.Word, "English", (long)information.Frequency);
            }

            return instance;
        }
    }
}
