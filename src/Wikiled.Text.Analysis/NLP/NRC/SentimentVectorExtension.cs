using System.Linq;
using Wikiled.Text.Analysis.Reflection;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    public static class SentimentVectorExtension
    {
        private static readonly IMapCategory MapProbabilityOnly = new CategoriesMapper().Construct<ItemProbabilityHolder>();

        public static DataTree GetTree(this SentimentVector vector, bool occurences = true)
        {
            var items = occurences ? vector.GetOccurences() : vector.GetProbabilities();
            return new DataTree(new ItemProbabilityHolder(items.ToArray()), MapProbabilityOnly);
        }

    }
}
