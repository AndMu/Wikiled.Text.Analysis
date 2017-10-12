using System.Collections.Generic;

namespace Wikiled.Text.Analysis.SymSpell
{
    public interface ISymSpell
    {
        List<SuggestItem> Lookup(string input, string language, int editDistanceMax);
    }
}