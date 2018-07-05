using System;
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
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("message", nameof(name));
            }

            Name = name;
            this.streamSource = streamSource ?? throw new ArgumentNullException(nameof(streamSource));
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
