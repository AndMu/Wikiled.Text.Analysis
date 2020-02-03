using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public static class ExtensionMethods
    {
        public static IEnumerable<WordVector> GetDocumentVector(this IWordModel model, Document document)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (document == null) throw new ArgumentNullException(nameof(document));


            foreach (var word in document.Words)
            {
                yield return GetWordVector(model, word);
            }

        }

        public static float[] GetParagraphVector(this IWordModel model, params SentenceItem[] sentences)
        {
            if (model == null)
            {
                throw new ArgumentNullException(nameof(model));
            }

            if (sentences == null)
            {
                throw new ArgumentNullException(nameof(sentences));
            }

            if (sentences.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(sentences));
            }

            var words = new ConcurrentBag<WordVector>();
            Parallel.ForEach(sentences.SelectMany(item => item.Words),
                word =>
                {
                    var result = GetWordVector(model, word);
                    if (result != null)
                    {
                        words.Add(result);
                    }
                });

            var arrays = words.ToArray();
            if (arrays.Length == 0)
            {
                return new float[model.Size];
            }

            return arrays.Average();
        }

        public static float[] Average(this WordVector[] vectors)
        {
            if (vectors == null)
            {
                throw new ArgumentNullException(nameof(vectors));
            }

            if (vectors.Length == 0)
            {
                throw new ArgumentException("Value cannot be an empty collection.", nameof(vectors));
            }

            var result = new float[vectors[0].Vector.Length];
            Parallel.For(0, result.Length, i => { result[i] = vectors.Sum(item => item.Vector[i]) / vectors.Length; });
            return result;
        }

        public static float[] Add(this float[] value1, float[] value2)
        {
            if (value1 == null)
            {
                throw new ArgumentNullException(nameof(value1));
            }

            if (value2 == null)
            {
                throw new ArgumentNullException(nameof(value2));
            }

            if (value1.Length != value2.Length)
            {
                throw new ArgumentException("vector lengths do not match");
            }

            var result = new float[value1.Length];
            Parallel.For(0, value1.Length, i => { result[i] = value1[i] + value2[i]; });
            return result;
        }

        public static float[] Subtract(this float[] value1, float[] value2)
        {
            if (value1 == null)
            {
                throw new ArgumentNullException(nameof(value1));
            }

            if (value2 == null)
            {
                throw new ArgumentNullException(nameof(value2));
            }

            if (value1.Length != value2.Length)
            {
                throw new ArgumentException("vector lengths do not match");
            }

            var result = new float[value1.Length];
            Parallel.For(0, value1.Length, i => { result[i] = value1[i] - value2[i]; });
            return result;
        }

        public static double Distance(this float[] value1, float[] value2)
        {
            if (value1 == null)
            {
                throw new ArgumentNullException(nameof(value1));
            }

            if (value2 == null)
            {
                throw new ArgumentNullException(nameof(value2));
            }

            if (value1.Length != value2.Length)
            {
                throw new ArgumentException("vector lengths do not match");
            }

            return Math.Sqrt(value1.Subtract(value2).Select(x => x * x).Sum());
        }


        public static WordVector GetByWord(this IWordModel model, string word)
        {
            return model.Vectors.FirstOrDefault(x => x.Word == word);
        }

        public static IEnumerable<WordVector> Nearest(this IWordModel model, float[] vector)
        {
            return model.Vectors.OrderBy(x => x.Vector.Distance(vector));
        }

        public static WordVector NearestSingle(this IWordModel model, float[] vector)
        {
            return model.Vectors.OrderBy(x => x.Vector.Distance(vector)).First();
        }

        public static double Distance(this IWordModel model, string word1, string word2)
        {
            var vector1 = model.GetByWord(word1);
            var vector2 = model.GetByWord(word2);
            if (vector1 == null)
            {
                throw new ArgumentException($"cannot find word1 '{word1}'");
            }

            if (vector2 == null)
            {
                throw new ArgumentException($"cannot find word2 '{word2}'");
            }

            return vector1.Vector.Distance(vector2.Vector);
        }

        public static IEnumerable<WordDistance> Nearest(this IWordModel model, string word)
        {
            var vector = model.GetByWord(word);
            if (vector == null)
            {
                throw new ArgumentException($"cannot find word '{word}'");
            }

            return model.Vectors.AsParallel()
                .Select(x => new WordDistance(x.Word, x.Vector.Distance(vector.Vector)))
                .OrderBy(x => x.Distance)
                .Where(x => x.Word != word);
        }

        public static double Distance(this WordVector word1, WordVector word2)
        {
            return word1.Vector.Distance(word2.Vector);
        }

        public static float[] Add(this WordVector word1, WordVector word2)
        {
            return word1.Vector.Add(word2.Vector);
        }

        public static float[] Subtract(this WordVector word1, WordVector word2)
        {
            return word1.Vector.Subtract(word2.Vector);
        }

        public static float[] Add(this float[] word1, WordVector word2)
        {
            return word1.Add(word2.Vector);
        }

        public static float[] Subtract(this float[] word1, WordVector word2)
        {
            return word1.Subtract(word2.Vector);
        }

        public static double Distance(this float[] word1, WordVector word2)
        {
            return word1.Distance(word2.Vector);
        }


        private static WordVector GetWordVector(IWordModel model, WordEx word)
        {
            if (!string.IsNullOrEmpty(word.Text))
            {
                var result = model.Find(word.Text);
                if (result != null)
                {
                    return result;
                }
            }

            if (!string.IsNullOrEmpty(word.Raw) && word.Raw != word.Text)
            {
                var result = model.Find(word.Raw);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
    }
}