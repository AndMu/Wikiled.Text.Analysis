using System.IO;
using Wikiled.Core.Utility.Arguments;
using Wikiled.Core.Utility.Resources;

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
