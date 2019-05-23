using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wikiled.MachineLearning.Mathematics.Vectors;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Similarity
{
    public class SimilarityDetector
    {
        private readonly IWordVectorEncoder encoder;

        private readonly IDistance distanceMeasurer;

        private readonly ILogger<SimilarityDetector> logger;

        private readonly Dictionary<IBagOfWords, VectorData> vectorTable = new Dictionary<IBagOfWords,VectorData>();

        public SimilarityDetector(ILogger<SimilarityDetector> logger, IWordVectorEncoder encoder, IDistance distanceMeasurer)
        {
            this.encoder = encoder ?? throw new ArgumentNullException(nameof(encoder));
            this.distanceMeasurer = distanceMeasurer ?? throw new ArgumentNullException(nameof(distanceMeasurer));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public void Register(IBagOfWords bag)
        {
            if (bag == null)
            {
                throw new ArgumentNullException(nameof(bag));
            }

            foreach (var bagWord in bag.Words)
            {
                encoder.AddWord(bagWord.Text);
            }

            vectorTable[bag] = null;
        }

        public IEnumerable<SimilarityResult> FindSimilar(IBagOfWords bag)
        {
            logger.LogDebug("Searching for similar documents");
            var vector = encoder.GetFullVector(bag.Words.Select(item => item.Text).ToArray());
            var distanceTable = new Dictionary<IBagOfWords, double?>();
            foreach (var existing in vectorTable)
            {
                distanceTable[existing.Key] = null;
            }

            Parallel.ForEach(vectorTable.Keys.ToArray(),
                             existingDocument =>
                             {
                                 var existing = vectorTable[existingDocument];
                                 if (existing == null)
                                 {
                                     existing = encoder.GetFullVector(existingDocument.Words.Select(item => item.Text).ToArray());
                                     vectorTable[existingDocument] = existing;
                                 }

                                 var distance = distanceMeasurer.Measure(vector, existing);
                                 distanceTable[existingDocument] = distance;
                             });

            return distanceTable.OrderByDescending(item => item.Value)
                .Where(item => item.Value.HasValue)
                .Select(item => new SimilarityResult(item.Key, item.Value.Value));
        }
    }
}
