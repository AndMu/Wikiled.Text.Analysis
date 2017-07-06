namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class FrequencyListManager
    {
        private FrequencyListManager()
        {
            Internet = new WordFrequencyList("BNC", @"Resources.Frequency.internet.dat");
            Reuters = new WordFrequencyList("BNC", @"Resources.Frequency.Reuters.dat");
            Subtitles = new WordFrequencyList("BNC", @"Resources.Frequency.subtitles.dat");
            BNC = new BNCList();
        }

        public static FrequencyListManager Instance { get; } = new FrequencyListManager();

        public BNCList BNC { get; }

        public WordFrequencyList Internet { get; }

        public WordFrequencyList Reuters { get; }

        public WordFrequencyList Subtitles { get; }
    }
}
