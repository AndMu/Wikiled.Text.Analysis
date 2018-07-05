using System;
using Microsoft.Extensions.Caching.Memory;
using Wikiled.Common.Arguments;
using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.WordNet.Engine;

namespace Wikiled.Text.Analysis.WordNet.InformationContent
{
    public class JcnMeasure : IRelatednessMesaure
    {
        private readonly IMemoryCache cache;

        private readonly IWordNetEngine engine;

        private readonly IInformationContentResnik resnik;

        public JcnMeasure(IMemoryCache cache, IInformationContentResnik resnik, IWordNetEngine engine)
        {
            Guard.NotNull(() => cache, cache);
            Guard.NotNull(() => resnik, resnik);
            Guard.NotNull(() => engine, engine);
            this.resnik = resnik;
            this.engine = engine;
            this.cache = cache;
        }

        public static double MaxSimilarity => 1 / MinDistance;

        public static double MinDistance => 1;

        public double Measure(string word1, string word2, WordType type = WordType.Noun)
        {
            if (type != WordType.Noun &&
                type != WordType.Verb)
            {
                throw new ArgumentOutOfRangeException(nameof(type));
            }

            if (string.IsNullOrWhiteSpace(word1))
            {
                throw new ArgumentNullException(nameof(word1));
            }

            if (string.IsNullOrWhiteSpace(word2))
            {
                throw new ArgumentNullException(nameof(word2));
            }

            if (string.Compare(word1, word2, StringComparison.OrdinalIgnoreCase) == 0)
            {
                return MaxSimilarity;
            }

            var synsets1 = engine.GetSynSets(word1, type);
            var synsets2 = engine.GetSynSets(word2, type);

            double relatedness = 0;
            foreach (var synSet1 in synsets1)
            {
                if (synsets2.Contains(synSet1))
                {
                    return MaxSimilarity;
                }
            }

            foreach (var synSet1 in synsets1)
            {
                foreach (var synSet2 in synsets2)
                {
                    var current = Measure(synSet1, synSet2);
                    if (current > relatedness)
                    {
                        relatedness = current;
                    }
                }
            }

            return relatedness;
        }

        public double Measure(SynSet synSet1, SynSet synSet2)
        {
            Guard.NotNull(() => synSet1, synSet1);
            Guard.NotNull(() => synSet2, synSet2);
            var tag = GenerateTag(synSet1, synSet2);
            return cache.GetOrCreate(
                tag,
                entry =>
                    {
                        SynSetRelation[] vlist = new SynSetRelation[1];
                        vlist[0] = SynSetRelation.Hypernym;
                        var common = synSet1.GetClosestMutuallyReachableSynset(synSet2, vlist);
                        double distance = 0;
                        if (common != null)
                        {
                            distance = resnik.GetIC(synSet1) + resnik.GetIC(synSet2) - 2 * resnik.GetIC(common);
                        }

                        if (distance == 0)
                        {
                            return 0;
                        }

                        if (distance < MinDistance)
                        {
                            distance = MinDistance;
                        }

                        return 1 / distance;
                    });
        }

        private string GenerateTag(SynSet synSet1, SynSet synSet2)
        {
            if (synSet1.Offset > synSet2.Offset)
            {
                return synSet1.Offset + ":" + synSet2.Offset;
            }

            return synSet2.Offset + ":" + synSet1.Offset;
        }
    }
}
