using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Tokenizer
{
    public interface IPipeline<T>
    {
        IEnumerable<T> Process(IEnumerable<T> words);
    }
}
