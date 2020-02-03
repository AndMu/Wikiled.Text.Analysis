using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Wikiled.Common.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class BinaryModelReader : IModelReader
    {
        public BinaryModelReader(Stream stream, bool lineBreaks = false, bool leaveOpen = false)
        {
            Stream = stream;
        }

        public bool LineBreaks { get; private set; }

        private Stream Stream { get; }

        public bool CaseSensitive { get; set; } = true;

        public IWordModel Open()
        {
            var vectors = new List<WordVector>();
            using (var reader = new BinaryReader(Stream, Encoding.UTF8, true))
            {
                int[] header = ReadHeader(reader);
                int words = header[0];
                int size = header[1];

                for (int i = 0; i < words; i++)
                {
                    WordVector vector;
                    while ((vector = ReadVector(reader, size)) == null)
                    {
                    }

                    vectors.Add(vector);
                }

                return new WordModel(ApplicationLogging.CreateLogger<WordModel>(), words, size, vectors, CaseSensitive);
            }
        }


        private int[] ReadHeader(BinaryReader reader)
        {
            int words = int.Parse(ReadString(reader), CultureInfo.InvariantCulture);
            int size = int.Parse(ReadString(reader), CultureInfo.InvariantCulture);

            return new[] { words, size };
        }

        private WordVector ReadVector(BinaryReader binaryReader, int size)
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

            var result = new WordVector(word, vector);
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