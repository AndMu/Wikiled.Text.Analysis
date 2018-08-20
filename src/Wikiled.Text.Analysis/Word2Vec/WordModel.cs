using System;
using System.Collections.Generic;
using NLog;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class WordModel
    {
        private static readonly ILogger logger = LogManager.GetCurrentClassLogger();

        private readonly Dictionary<string, WordVector> vectorsTable;

        public WordModel(int words, int size, List<WordVector> vectors)
        {
            Words = words;
            Size = size;
            vectorsTable = new Dictionary<string, WordVector>();
            if (words != vectors.Count)
            {
                throw new ArgumentOutOfRangeException("Size mistmatch");
            }

            foreach (var wordVector in vectors)
            {
                if (vectorsTable.ContainsKey(wordVector.Word))
                {
                    logger.Warn("Word already added: " + wordVector.Word);
                }

                vectorsTable[wordVector.Word] = wordVector;
            }
        }

        public int Words { get; }

        public int Size { get; }

        public IEnumerable<WordVector> Vectors => vectorsTable.Values;


        public WordVector Find(string word)
        {
            vectorsTable.TryGetValue(word, out var result);
            return result;
        }

        protected void AddVector(WordVector vector)
        {
            vectorsTable.Add(vector.Word, vector);
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