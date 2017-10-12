using System.Collections.Generic;

namespace Wikiled.Text.Analysis.SymSpell
{
    public interface ISymSpellCompound
    {
        List<SuggestItem> Lookup(string input, int editDistanceMax);

        List<SuggestItem> LookupCompound(string input, int editDistanceMax);
    }
}