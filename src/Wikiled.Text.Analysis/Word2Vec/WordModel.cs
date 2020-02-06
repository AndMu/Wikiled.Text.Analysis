using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class WordModel : IWordModel
    {
        private readonly ILogger<WordModel> logger;

        private readonly Dictionary<string, WordVector> vectorsTable;

        public WordModel(ILogger<WordModel> logger, int size, IEnumerable<WordVector> vectors, bool caseSensitive)
        {
            Size = size;
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.CaseSensitive = caseSensitive;
            vectorsTable = new Dictionary<string, WordVector>(caseSensitive ? StringComparer.CurrentCulture : StringComparer.CurrentCultureIgnoreCase);

            foreach (var wordVector in vectors)
            {
                if (vectorsTable.ContainsKey(wordVector.Word))
                {
                    this.logger.LogWarning("Word already added: " + wordVector.Word);
                }

                vectorsTable[wordVector.Word] = wordVector;
            }
        }

        public bool CaseSensitive { get; }

        public int Words => vectorsTable.Count;

        public int Size { get; }

        public IEnumerable<WordVector> Vectors => vectorsTable.Values;

        public static IWordModel Load(string filename)
        {
            return new ModelReaderFactory(ApplicationLogging.LoggerFactory).Construct(filename);
        }

        public static IWordModel Load(IModelReader source)
        {
            return source.Open();
        }

        public WordVector Find(string word)
        {
            vectorsTable.TryGetValue(word, out var result);
            return result;
        }

        protected void AddVector(WordVector vector)
        {
            vectorsTable.Add(vector.Word, vector);
        }
    }
}