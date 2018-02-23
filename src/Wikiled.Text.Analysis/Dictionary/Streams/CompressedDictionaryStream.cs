using System.IO;
using Wikiled.Common.Arguments;
using Wikiled.Common.Helpers;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class CompressedDictionaryStream : IDictionaryStream
    {
        private readonly IStreamSource streamSource;

        public CompressedDictionaryStream(string name, IStreamSource streamSource)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNull(() => streamSource, streamSource);
            Name = name;
            this.streamSource = streamSource;
        }

        public string Name { get; }

        public TextReader ConstructReadStream()
        {
            using (BinaryReader reader = new BinaryReader(streamSource.ConstructReader(Name)))
            {
                byte[] data = new byte[reader.BaseStream.Length];
                reader.Read(data, 0, data.Length);
                var unzipedText = data.UnZipString();
                return new StringReader(unzipedText);
            }
        }
    }
}
