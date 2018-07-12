using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    public interface INRCDictionary
    {
        IEnumerable<WordNRCRecord> AllRecords { get; }

        void Load();

        NRCRecord FindRecord(string word);

        NRCRecord FindRecord(WordEx word);
    }
}