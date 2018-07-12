using System;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class FrequencyListManager : IFrequencyListManager
    {
        private readonly Lazy<WordFrequencyList> common = new Lazy<WordFrequencyList>(() => new WordFrequencyList("BNC", @"Resources.Frequency.frequency.dat"));

        private readonly Lazy<WordFrequencyList> internet = new Lazy<WordFrequencyList>(() => new WordFrequencyList("BNC", @"Resources.Frequency.internet.dat"));

        private readonly Lazy<WordFrequencyList> reuters = new Lazy<WordFrequencyList>(() => new WordFrequencyList("BNC", @"Resources.Frequency.Reuters.dat"));

        private readonly Lazy<WordFrequencyList> subtitles = new Lazy<WordFrequencyList>(() => new WordFrequencyList("BNC", @"Resources.Frequency.subtitles.dat"));

        private readonly Lazy<BNCList> bnc = new Lazy<BNCList>(() => new BNCList());

        public WordFrequencyList Common => common.Value;

        public BNCList BNC => bnc.Value;

        public WordFrequencyList Internet => internet.Value;

        public WordFrequencyList Reuters => reuters.Value;

        public WordFrequencyList Subtitles => subtitles.Value;
    }
}
