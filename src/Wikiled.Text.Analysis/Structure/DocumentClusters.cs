using System;

namespace Wikiled.Text.Analysis.Structure
{
    public class DocumentClusters
    {
        public DocumentClusters(IProcessingTextBlock[] clusters)
        {
            Clusters = clusters ?? throw new ArgumentNullException(nameof(clusters));
        }

        public IProcessingTextBlock[] Clusters { get; }
    }
}
