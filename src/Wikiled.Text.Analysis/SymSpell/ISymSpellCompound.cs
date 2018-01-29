using System.Collections.Generic;

namespace Wikiled.Text.Analysis.SymSpell
{
    public interface ISymSpellCompound
    {
        int Verbose { get; set; }

        int EditDistanceMax { get; set; }

        List<SuggestItem> Lookup(string input, int editDistanceMax = 2);

        List<SuggestItem> LookupCompound(string input, int editDistanceMax = 2);
    }
}