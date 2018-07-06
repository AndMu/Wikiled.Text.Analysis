using System;
using System.Collections.Generic;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.Reflection
{
    [InfoCategory("Wrapper")]
    public class ItemProbabilityHolder
    {
        public ItemProbabilityHolder(IList<IItemProbability<string>> probabilities)
        {
            Probabilities = probabilities ?? throw new ArgumentNullException(nameof(probabilities));
        }

        [InfoArrayCategory("List", "Data", "Probability")]
        public IList<IItemProbability<string>> Probabilities { get; }
    }
}
