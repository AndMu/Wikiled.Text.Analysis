using System.IO;
using Wikiled.Common.Arguments;
using Wikiled.Common.Resources;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class EmbeddedStreamSource<T> : IStreamSource
    {
        public Stream ConstructReader(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);
            return typeof(T).GetEmbeddedFile(name);
        }
    }
}
