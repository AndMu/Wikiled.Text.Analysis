using System;
using System.IO;
using Wikiled.Common.Resources;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class EmbeddedStreamSource<T> : IStreamSource
    {
        public Stream ConstructReader(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            return typeof(T).GetEmbeddedFile(name);
        }
    }
}
