using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class WordCleanupPipeline : IPipeline<string>
    {
        public IEnumerable<string> Process(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                string currentWord = word;
                if (string.IsNullOrEmpty(currentWord))
                {
                    continue;
                }

                currentWord = currentWord.TrimEnd('\r', '\n');
                yield return currentWord;
            }
        }
    }
}
