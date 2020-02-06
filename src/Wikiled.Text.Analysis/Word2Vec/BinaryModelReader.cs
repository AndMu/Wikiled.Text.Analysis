using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class BinaryModelReader : IModelReader
    {
        private readonly ILoggerFactory loggerFactory;

        private readonly bool leaveOpen;

        private readonly ILogger<BinaryModelReader> logger;

        public BinaryModelReader(ILoggerFactory loggerFactory, Stream stream, bool lineBreaks = false, bool leaveOpen = false)
        {
            Stream = stream ?? throw new ArgumentNullException(nameof(stream));
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
            this.leaveOpen = leaveOpen;
            LineBreaks = lineBreaks;
        }

        public bool LineBreaks { get; }

        private Stream Stream { get; }

        public bool CaseSensitive { get; set; } = true;

        public IWordModel Open()
        {
            using var reader = new BinaryReader(Stream, Encoding.UTF8, leaveOpen);
            int[] header = ReadHeader(reader);
            var words = header[0];
            var size = header[1];

            IEnumerable<WordVector> Populate()
            {
                for (int i = 0; i < words; i++)
                {
                    WordVector vector;
                    while ((vector = ReadVector(reader, size, i)) == null)
                    {
                    }

                    yield return vector;
                }
            }

            var result = new WordModel(loggerFactory.CreateLogger<WordModel>(), size, Populate(), CaseSensitive);
            if (words != result.Words)
            {
                logger.LogWarning("Mismatch in word count. Expected {0} and got {1}", words, result.Words);
            }

            return result;
        }


        private int[] ReadHeader(BinaryReader reader)
        {
            int words = int.Parse(ReadString(reader), CultureInfo.InvariantCulture);
            int size = int.Parse(ReadString(reader), CultureInfo.InvariantCulture);

            return new[] { words, size };
        }

        private WordVector ReadVector(BinaryReader binaryReader, int size, int index)
        {
            string word = ReadString(binaryReader);
            if (string.IsNullOrEmpty(word))
            {
                return null;
            }

            float[] vector = new float[size];

            for (int j = 0; j < size; j++)
            {
                vector[j] = binaryReader.ReadSingle();
            }

            var result = new WordVector(index, word, vector);
            if (LineBreaks)
            {
                binaryReader.ReadByte(); // consume line break
            }

            return result;
        }

        private static bool IsStringEnd(char[] c)
        {
            return c == null || c[0] == 32 || c[0] == 10;
        }

        private static char[] ReadUTF8Char(Stream stream)
        {
            int byteAsInt = 0;
            Decoder decoder = Encoding.UTF8.GetDecoder();
            char[] nextChar = new char[2];

            while ((byteAsInt = stream.ReadByte()) != -1)
            {
                int charCount = decoder.GetChars(new[] { (byte)byteAsInt }, 0, 1, nextChar, 0);
                if (charCount == 0)
                {
                    continue;
                }

                return nextChar.Take(charCount).ToArray();
            }

            return null;
        }

        private string ReadString(BinaryReader binaryReader)
        {
            var sb = new StringBuilder();
            char[] c;
            while (!IsStringEnd(c = ReadUTF8Char(binaryReader.BaseStream)))
            {
                sb.Append(c);
            }

            return sb.ToString();
        }
    }
}