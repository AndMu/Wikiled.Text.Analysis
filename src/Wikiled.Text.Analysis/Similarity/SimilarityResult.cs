using System;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Similarity
{
    public class SimilarityResult
    {
        public SimilarityResult(IBagOfWords document, double similarity)
        {
            Document = document ?? throw new ArgumentNullException(nameof(document));
            Similarity = similarity;
        }

        public IBagOfWords Document { get; }

        public double Similarity { get; }
    }
}
