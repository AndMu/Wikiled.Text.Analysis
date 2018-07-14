using System.Collections.Generic;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.NLP.NRC
{
    public interface INRCDictionary
    {
        IEnumerable<WordNRCRecord> AllRecords { get; }

        void Load();

        SentimentVector Extract(IEnumerable<WordEx> words);

        NRCRecord FindRecord(string word);

        NRCRecord FindRecord(WordEx word);
    }
}