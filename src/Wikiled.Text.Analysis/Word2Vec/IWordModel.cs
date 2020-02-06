using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IWordModel
    {
        bool CaseSensitive { get; }

        int Words { get; }

        int Size { get; }

        IEnumerable<WordVector> Vectors { get; }

        WordVector Find(string word);
    }
}