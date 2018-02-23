using System.Collections.Generic;
using Wikiled.Common.Arguments;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.Reflection
{
    [InfoCategory("Wrapper")]
    public class ItemProbabilityHolder
    {
        public ItemProbabilityHolder(IList<IItemProbability<string>> probabilities)
        {
            Guard.NotNull(() => probabilities, probabilities);
            Probabilities = probabilities;
        }

        [InfoArrayCategory("List", "Data", "Probability")]
        public IList<IItemProbability<string>> Probabilities { get; }
    }
}
