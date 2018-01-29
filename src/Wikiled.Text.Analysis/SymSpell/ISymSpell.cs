using System.Collections.Generic;

namespace Wikiled.Text.Analysis.SymSpell
{
    public interface ISymSpell
    {
        int Verbose { get; set; }

        int EditDistanceMax { get; set; }

        List<SuggestItem> Lookup(string input, int editDistance = 2);
    }
}