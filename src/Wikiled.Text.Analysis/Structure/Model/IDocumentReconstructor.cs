using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure.Model
{
    public interface IDocumentReconstructor
    {
        Document Reconstruct(ICollection<SentenceItem> sentences);
    }
}
