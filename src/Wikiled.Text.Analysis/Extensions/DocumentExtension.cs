using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Extensions
{
    public static class DocumentExtension
    {
        public static string GetTextId(this IDocument document)
        {
            return $"Text:{document.Text.GenerateKey()}";
        }

        public static string GetId(this IDocument document)
        {
            return $"Document:{document.Id}:{document.Text.GenerateKey()}";
        }

        public static LightDocument GetLight(this Document document)
        {
            var result = new LightDocument();
            result.Text = document.Text;
            result.Author = document.Author;
            result.DocumentTime = document.DocumentTime;
            result.Id = result.Id;
            result.Title = result.Title;
            foreach (var sentence in document.Sentences)
            {
                var resultSentence = new LightSentence();
                resultSentence.Text = sentence.Text;
                result.Sentences.Add(resultSentence);
                foreach (var word in sentence.Words)
                {
                    var resultWord = new LightWord();
                    resultWord.Text = word.Text;
                    resultWord.Tag = word.Type;
                    resultWord.Phrase = word.Phrase;
                    resultSentence.Words.Add(resultWord);
                }
            }

            return result;
        }
    }
}
