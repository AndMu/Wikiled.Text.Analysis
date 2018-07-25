using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class Model
    {
        private readonly List<WordVector> vectors;

        public Model(int words, int size, List<WordVector> vectors)
        {
            Words = words;
            Size = size;
            this.vectors = vectors;
        }

        public int Words { get; }

        public int Size { get; }

        public IEnumerable<WordVector> Vectors => vectors;

        protected void AddVector(WordVector vector)
        {
            vectors.Add(vector);
        }

        public static Model Load(string filename)
        {
            return new ModelReaderFactory().Manufacture(filename);
        }

        public static Model Load(IModelReader source)
        {
            return source.Open();
        }
    }
}