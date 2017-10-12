using System.Collections.Generic;

namespace Wikiled.Text.Analysis.SymSpell
{
    public interface ISymSpell
    {
        List<SuggestItem> Lookup(string input, SuggestionType type = SuggestionType.Top);
    }
}