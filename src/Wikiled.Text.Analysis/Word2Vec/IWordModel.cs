using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IWordModel
    {
        int Words { get; }

        int Size { get; }

        IEnumerable<WordVector> Vectors { get; }

        WordVector Find(string word);
    }
}