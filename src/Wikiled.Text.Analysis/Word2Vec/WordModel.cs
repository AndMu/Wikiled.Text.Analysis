using System.Collections.Generic;
using System.Linq;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class WordModel
    {
        private readonly Dictionary<string, WordVector> vectors;

        public WordModel(int words, int size, List<WordVector> vectors)
        {
            Words = words;
            Size = size;
            this.vectors = vectors.ToDictionary(item => item.Word, item => item);
        }

        public int Words { get; }

        public int Size { get; }

        public IEnumerable<WordVector> Vectors => vectors.Values;


        public WordVector Find(string word)
        {
            vectors.TryGetValue(word, out var result);
            return result;
        }

        protected void AddVector(WordVector vector)
        {
            vectors.Add(vector.Word, vector);
        }

        public static WordModel Load(string filename)
        {
            return new ModelReaderFactory().Manufacture(filename);
        }

        public static WordModel Load(IModelReader source)
        {
            return source.Open();
        }
    }
}