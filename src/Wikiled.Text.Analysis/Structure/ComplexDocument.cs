using System;
using System.Linq;

namespace Wikiled.Text.Analysis.Structure
{
    public class ComplexDocument
    {
        public ComplexDocument(params Document[] document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            if (document.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(document));
            }

            Pages = document;
            Sentences = Pages.SelectMany(item => item.Sentences).ToArray();

            // reindex using global index
            int index = 0;
            foreach (var page in Pages)
            {
                foreach (var pageSentence in page.Sentences)
                {
                    pageSentence.Index = index;
                    index++;
                }
            }
        }

        public Document[] Pages { get; }

        public SentenceItem[] Sentences { get; }
    }
}
