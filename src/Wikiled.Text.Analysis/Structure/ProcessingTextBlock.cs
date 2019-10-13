using System;
using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure
{
    public class ProcessingTextBlock : IProcessingTextBlock
    {
        public ProcessingTextBlock(params SentenceItem[] sentences)
        {
            if (sentences == null)
            {
                throw new ArgumentNullException(nameof(sentences));
            }

            Sentences = new List<SentenceItem>(sentences);
        }

        public List<SentenceItem> Sentences { get; }
    }
}
