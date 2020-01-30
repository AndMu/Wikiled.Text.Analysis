using System.IO;
using System.Text;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class BinaryModelWriter : IModelWriter
    {
        public BinaryModelWriter(Stream s, bool leaveOpen = false)
        {
            Writer = new BinaryWriter(s, Encoding.UTF8, leaveOpen);
        }

        private BinaryWriter Writer { get; }

        public void Write(IWordModel model)
        {
            WriteHeader(model);
            foreach (var wv in model.Vectors)
            {
                WriteWordVector(wv);
            }
        }

        public void Dispose()
        {
            Writer.Dispose();
        }

        private void WriteHeader(IWordModel m)
        {
            WriteString($"{m.Words} {m.Size}\n");
        }

        private void WriteWordVector(WordVector wv)
        {
            WriteString(wv.Word);
            WriteString(" ");
            foreach (var f in wv.Vector)
            {
                Writer.Write(f);
            }
        }

        private void WriteString(string s)
        {
            var enc = new UTF8Encoding(false, true);
            Writer.Write(enc.GetBytes(s));
        }
    }
}