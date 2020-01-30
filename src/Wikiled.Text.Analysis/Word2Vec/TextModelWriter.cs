using System.IO;
using System.Text;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class TextModelWriter : IModelWriter
    {
        public TextModelWriter(Stream stream, bool leaveOpen = false, int bufferSize = 4096)
        {
            Writer = new StreamWriter(stream, Encoding.UTF8, bufferSize, leaveOpen);
        }

        private StreamWriter Writer { get; }

        public void Write(IWordModel model)
        {
            //Write the header
            WriteHeader(model);

            //Write the vectors
            foreach (var wv in model.Vectors)
            {
                WriteWordVector(wv);
            }
        }

        public void Dispose()
        {
            Writer.Dispose();
        }

        private void WriteWordVector(WordVector wv)
        {
            Writer.Write(wv.Word);
            Writer.Write(' ');
            Writer.Write(string.Join(" ", wv.Vector));
            Writer.Write('\n');
        }

        private void WriteHeader(IWordModel m)
        {
            Writer.Write(m.Words);
            Writer.Write(' ');
            Writer.Write(m.Size);
            Writer.Write('\n');
        }
    }
}