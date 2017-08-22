using System.Collections.Generic;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    public interface INRCDictionary
    {
        IEnumerable<WordNRCRecord> AllRecords { get; }

        void Load();

        NRCRecord FindRecord(string word);
    }
}