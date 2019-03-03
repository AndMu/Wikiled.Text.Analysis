using System;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class FrequencyListManager : IFrequencyListManager
    {
        private readonly Lazy<WordFrequencyList> common = new Lazy<WordFrequencyList>(
            () => new WordFrequencyList(
                "Common",
                new CompressedDictionaryStream(
                    @"Resources.Frequency.frequency.dat",
                    new EmbeddedStreamSource<FrequencyListManager>())));

        private readonly Lazy<WordFrequencyList> internet = new Lazy<WordFrequencyList>(
            () => new WordFrequencyList(
                "Internet",
                new CompressedDictionaryStream(
                    @"Resources.Frequency.internet.dat",
                    new EmbeddedStreamSource<FrequencyListManager>())));

        private readonly Lazy<WordFrequencyList> reuters = new Lazy<WordFrequencyList>(
                () => new WordFrequencyList(
                    "Reuters",
                    new CompressedDictionaryStream(
                        @"Resources.Frequency.Reuters.dat",
                        new EmbeddedStreamSource<FrequencyListManager>())));

        private readonly Lazy<WordFrequencyList> subtitles = new Lazy<WordFrequencyList>(
            () => new WordFrequencyList(
                "subtitles",
                new CompressedDictionaryStream(
                    @"Resources.Frequency.subtitles.dat",
                    new EmbeddedStreamSource<FrequencyListManager>())));

        private readonly Lazy<BNCList> bnc = new Lazy<BNCList>(() => new BNCList());

        public WordFrequencyList Common => common.Value;

        public BNCList BNC => bnc.Value;

        public WordFrequencyList Internet => internet.Value;

        public WordFrequencyList Reuters => reuters.Value;

        public WordFrequencyList Subtitles => subtitles.Value;
    }
}
