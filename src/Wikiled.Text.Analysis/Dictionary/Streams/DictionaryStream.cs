using System.Collections.Generic;
using System.IO;
using Wikiled.Core.Utility.Arguments;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class DictionaryStream : IDictionaryStream
    {
        private readonly IStreamSource streamSource;

        public DictionaryStream(string name, IStreamSource streamSource)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNull(() => streamSource, streamSource);
            Name = name;
            this.streamSource = streamSource;
        }

        public string Name { get; }

        public TextReader ConstructReadStream()
        {
            return new StreamReader(streamSource.ConstructReader(Name));
        }
    }
}
