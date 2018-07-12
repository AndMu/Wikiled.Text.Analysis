namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public interface IFrequencyListManager
    {
        BNCList BNC { get; }
        WordFrequencyList Common { get; }
        WordFrequencyList Internet { get; }
        WordFrequencyList Reuters { get; }
        WordFrequencyList Subtitles { get; }
    }
}