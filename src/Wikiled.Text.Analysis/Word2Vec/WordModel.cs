using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class WordModel
    {
        private readonly ILogger Logger;

        private readonly Dictionary<string, WordVector> vectorsTable;

        public WordModel(ILogger logger, int words, int size, List<WordVector> vectors)
        {
            Words = words;
            Size = size;
            Logger = logger ?? throw new ArgumentNullException(nameof(logger));
            vectorsTable = new Dictionary<string, WordVector>();
            if (words != vectors.Count)
            {
                throw new ArgumentOutOfRangeException("Size mistmatch");
            }

            foreach (var wordVector in vectors)
            {
                if (vectorsTable.ContainsKey(wordVector.Word))
                {
                    Logger.LogWarning("Word already added: " + wordVector.Word);
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