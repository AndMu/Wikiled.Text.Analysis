using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class WordModel : IWordModel
    {
        private readonly ILogger logger;

        private readonly Dictionary<string, WordVector> vectorsTable;

        public WordModel(ILogger logger, int words, int size, List<WordVector> vectors, bool caseSensitive)
        {
            Words = words;
            Size = size;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            vectorsTable = new Dictionary<string, WordVector>(caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase);
            if (words != vectors.Count)
            {
                throw new ArgumentOutOfRangeException("Size mistmatch");
            }

            foreach (var wordVector in vectors)
            {
                if (vectorsTable.ContainsKey(wordVector.Word))
                {
                    this.logger.LogWarning("Word already added: " + wordVector.Word);
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

        public static IWordModel Load(string filename)
        {
            return new ModelReaderFactory().Contruct(filename);
        }

        public static IWordModel Load(IModelReader source)
        {
            return source.Open();
        }
    }
}