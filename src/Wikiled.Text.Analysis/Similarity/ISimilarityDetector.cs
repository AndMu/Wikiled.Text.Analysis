using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Similarity
{
    public interface ISimilarityDetector
    {
        void Register(IBagOfWords bag);
        IEnumerable<SimilarityResult> FindSimilar(IBagOfWords bag);
    }
}