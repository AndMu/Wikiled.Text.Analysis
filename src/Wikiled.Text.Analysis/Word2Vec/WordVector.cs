namespace Wikiled.Text.Analysis.Word2Vec
{
    public class WordVector
    {
        public WordVector(int index, string word, float[] vector)
        {
            Index = index;
            Word = word;
            Vector = vector;
        }

        public int Index { get; }

        public string Word { get; }

        public int? Count { get; set; }

        public float[] Vector { get; }

        public override string ToString()
        {
            return Word;
        }

        public static float[] operator +(WordVector word1, WordVector word2)
        {
            return word1.Add(word2);
        }

        public static float[] operator -(WordVector word1, WordVector word2)
        {
            return word1.Subtract(word2);
        }

        public static float[] operator +(float[] word1, WordVector word2)
        {
            return word1.Add(word2);
        }

        public static float[] operator -(float[] word1, WordVector word2)
        {
            return word1.Subtract(word2);
        }

        public static float[] operator +(WordVector word1, float[] word2)
        {
            return word2.Add(word1);
        }

        public static float[] operator -(WordVector word1, float[] word2)
        {
            return word1.Vector.Subtract(word2);
        }
    }
}