using System.Collections.Generic;
using Wikiled.Text.Analysis.Tokenizer.Pipelined;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class LowerCasePipeline : IPipeline<string>
    {
        public IEnumerable<string> Process(IEnumerable<string> words)
        {
            foreach (var word in words)
            {
                if (!string.IsNullOrEmpty(word))
                {
                    yield return word.ToLower();
                }
            }
        }
    }
}
