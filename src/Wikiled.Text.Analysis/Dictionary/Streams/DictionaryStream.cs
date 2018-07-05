using System.IO;
using Wikiled.Common.Arguments;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class DictionaryStream : IDictionaryStream
    {
        private readonly IStreamSource streamSource;

        public DictionaryStream(string name, IStreamSource streamSource)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                throw new System.ArgumentException("message", nameof(name));
            }

            Name = name;
            this.streamSource = streamSource ?? throw new System.ArgumentNullException(nameof(streamSource));
        }

        public string Name { get; }

        public TextReader ConstructReadStream()
        {
            return new StreamReader(streamSource.ConstructReader(Name));
        }
    }
}
