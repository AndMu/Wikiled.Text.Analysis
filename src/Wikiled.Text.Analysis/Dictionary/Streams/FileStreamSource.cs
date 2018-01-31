﻿using System.IO;
using Wikiled.Core.Utility.Arguments;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public class FileStreamSource : IStreamSource
    {
        public Stream ConstructReader(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);
            return new FileStream(name, FileMode.Open);
        }
    }
}