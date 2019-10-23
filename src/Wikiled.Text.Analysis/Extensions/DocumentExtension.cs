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
            result.Sentences = new LightSentence[document.Sentences.Count];

            for (var i = 0; i < document.Sentences.Count; i++)
            {
                var sentence = document.Sentences[i];
                var resultSentence = new LightSentence();
                resultSentence.Text = sentence.Text;
                resultSentence.Words = new LightWord[sentence.Words.Count];

                for (var index = 0; index < sentence.Words.Count; index++)
                {
                    var word = sentence.Words[index];
                    var resultWord = new LightWord();
                    resultWord.Text = word.Text;
                    resultWord.Tag = word.Type;
                    resultWord.Phrase = word.Phrase;
                    resultSentence.Words[index] = resultWord;
                }

                result.Sentences[i] = resultSentence;
            }

            return result;
        }
    }
}
