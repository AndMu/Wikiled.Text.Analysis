using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public class InvertorPipeline : IPipeline<WordEx>
    {
        public IEnumerable<WordEx> Process(IEnumerable<WordEx> words)
        {
           int total = 0;
           bool invertor = false;
            foreach (var word in words)
            {
                total++;
                if (total >= 5 ||
                    word.IsConjunction())
                {
                    total = 0;
                    invertor = false;
                }

                if (word.IsInvertor)
                {
                    total = 0;
                    invertor = true;
                    continue;
                }

                if (invertor)
                {
                    var newResult = (WordEx)word.Clone();
                    newResult.Text = "not_" + newResult.Text;
                    newResult.IsInverted = true;
                    yield return newResult;
                }
                else
                {
                    yield return word;    
                }
            }
        }
    }
}
