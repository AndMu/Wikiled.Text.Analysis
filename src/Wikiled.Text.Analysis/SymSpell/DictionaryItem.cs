using System;
using System.Collections.Generic;

namespace Wikiled.Text.Analysis.SymSpell
{
    public class DictionaryItem
    {
        public List<int> Suggestions { get; } = new List<Int32>(2);

        public long Count { get; set; } = 0;
    }
}