using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure
{
    public interface IProcessingTextBlock
    {
        List<SentenceItem> Sentences { get; }
    }
}
