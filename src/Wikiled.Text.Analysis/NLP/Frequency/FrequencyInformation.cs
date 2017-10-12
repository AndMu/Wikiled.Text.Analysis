using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.NLP.Frequency
{
    public class FrequencyInformation
    {
        public FrequencyInformation(string word, int index, double frequency, BasePOSType pos = null)
        {
            Word = word;
            Index = index;
            Frequency = frequency;
            Pos = pos;
        }

        public string Word { get; }

        public int Index { get; }

        public double Frequency { get; }

        public BasePOSType Pos { get; }
    }
}
