using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class TextModelReader : IModelReader
    {
        private readonly Stream stream;

        private readonly ILoggerFactory loggerFactory;

        private readonly ILogger<TextModelReader> logger;

        public TextModelReader(ILoggerFactory loggerFactory, Stream stream)
        {
            this.stream = stream;
            this.loggerFactory = loggerFactory;
            logger = loggerFactory.CreateLogger<TextModelReader>();
        }

        public bool CaseSensitive { get; set; } = true;

        public IWordModel Open()
        {
            logger.LogDebug("Open");
            using var reader = new StreamReader(stream, Encoding.UTF8, true, 4 * 1024);
            var header = ReadHeader(reader);
            var words = header[0];
            var size = header[1];

            IEnumerable<WordVector> Populate()
            {
                WordVector vector;
                int count = 0;
                while ((vector = ReadVector(reader, count)) != null)
                {
                    yield return vector;
                    count++;
                }
            }

            var result = new WordModel(loggerFactory.CreateLogger<WordModel>(),
                size == 0 ? (int) stream.Length : size,
                Populate(),
                CaseSensitive);
            if (words != result.Words)
            {
                logger.LogWarning("Mismatch in word count. Expected {0} and got {1}", words, result.Words);
            }

            return result;
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

        private WordVector ReadVector(StreamReader reader, int index)
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
            return new WordVector(index, word, vector);
        }
    }
}