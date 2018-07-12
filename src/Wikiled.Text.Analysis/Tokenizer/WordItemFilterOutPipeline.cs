using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public class WordItemFilterOutPipeline : IPipeline<WordEx>
    {
        private readonly Func<WordEx, bool> condition;

        public WordItemFilterOutPipeline(Func<WordEx, bool> condition)
        {
            this.condition = condition ?? throw new ArgumentNullException(nameof(condition));
        }

        public IEnumerable<WordEx> Process(IEnumerable<WordEx> words)
        {
            return words.Where(word => !condition(word));
        }
    }
}
