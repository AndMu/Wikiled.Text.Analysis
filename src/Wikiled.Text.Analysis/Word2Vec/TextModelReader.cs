using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class TextModelReader : IModelReader
    {
        private readonly Stream stream;

        public TextModelReader(Stream stream)
        {
            this.stream = stream;
        }

        public WordModel Open()
        {
            using (var reader = new StreamReader(stream, Encoding.UTF8, true, 4 * 1024))
            {
                var header = ReadHeader(reader);
                var words = header[0];
                var size = header[1];

                var vectors = new List<WordVector>();
                WordVector vector = null;
                while (null != (vector = ReadVector(reader)))
                {
                    vectors.Add(vector);
                }

                return new WordModel(words == 0 ? vectors.Count : words, size == 0 ? (int)stream.Length : size, vectors);
            }
        }

        private int[] ReadHeader(StreamReader reader)
        {
            var headerLine = reader.ReadLine();

            var headerCount = headerLine.Split(' ').Length;

            if (headerCount == 2)
            {
                return headerLine.Split(' ').Select(x => int.Parse(x.Trim(), CultureInfo.InvariantCulture)).ToArray();
            }

            //this.Stream.Position = 0;
            return new[] { 0, 0 };
        }

        private WordVector ReadVector(StreamReader reader)
        {
            var line = reader.ReadLine();
            if (line == null)
            {
                return null;
            }

            var lineParts = line.Split(' ');
            var word = lineParts[0];
            var vector = lineParts.Skip(1)
                .Where(x => !string.IsNullOrWhiteSpace(x))
                .Select(x => float.Parse(x, CultureInfo.InvariantCulture))
                .ToArray();
            return new WordVector(word, vector);
        }
    }
}