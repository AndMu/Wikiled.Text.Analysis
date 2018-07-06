using System;
using System.IO;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class FileStreamSource : IStreamSource
    {
        public Stream ConstructReader(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            return new FileStream(name, FileMode.Open);
        }
    }
}
