using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class SimpleWordsExtraction : IWordsExtraction
    {
        private readonly ISentenceTokenizer tokenizer;

        public SimpleWordsExtraction(ISentenceTokenizer tokenizer)
        {
            this.tokenizer = tokenizer ?? throw new System.ArgumentNullException(nameof(tokenizer));
        }

        public Document GetDocument(string text)
        {
            Document document = new Document(text);
            foreach (var sentenceLevel in tokenizer.Parse(text))
            {
                ProcessSentence(document, sentenceLevel.SentenceText, sentenceLevel.GetWordItems());
            }

            return document;
        }

        private static void ProcessSentence(Document document, string sentence, IEnumerable<WordEx> words)
        {
            if (string.IsNullOrWhiteSpace(sentence))
            {
                return;
            }

            var currentSentence = new SentenceItem(sentence);
            foreach (WordEx item in words)
            {
                currentSentence.Add(item);
            }

            if (currentSentence.Words.Count > 0)
            {
                document.Add(currentSentence, false);
            }
        }
    }
}
