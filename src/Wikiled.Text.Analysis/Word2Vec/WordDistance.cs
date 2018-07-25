namespace Wikiled.Text.Analysis.Word2Vec
{
    public struct WordDistance
    {
        public WordDistance(string word, double distance)
        {
            Word = word;
            Distance = distance;
        }

        public string Word { get; }

        public double Distance { get; }

        public override string ToString()
        {
            return $"{Word} ({Distance})";
        }
    }
}