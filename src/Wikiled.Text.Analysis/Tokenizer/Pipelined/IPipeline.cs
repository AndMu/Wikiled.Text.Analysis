using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Tokenizer.Pipelined
{
    public interface IPipeline<T>
    {
        IEnumerable<T> Process(IEnumerable<T> words);
    }
}
