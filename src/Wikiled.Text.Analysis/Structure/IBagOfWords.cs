using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure
{
    public interface IBagOfWords
    {
        int TotalWords { get; }

        IEnumerable<WordEx> Words { get; }
    }
}