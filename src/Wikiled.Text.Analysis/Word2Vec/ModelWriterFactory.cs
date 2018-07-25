using System;
using System.IO;
using System.IO.Compression;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class ModelWriterFactory
    {
        public IModelWriter Manufacture(string filePath)
        {
            Stream fileStream = File.OpenWrite(filePath);
            var ext = Path.GetExtension(filePath);
            if (ext == ".gz")
            {
                fileStream = new GZipStream(fileStream, CompressionMode.Compress);
                ext = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath));
            }

            switch (ext)
            {
                case ".txt":
                    return new TextModelWriter(fileStream);
                case ".bin":
                    return new BinaryModelWriter(fileStream);
                default:
                    var error = new InvalidOperationException("Unrecognized file type");
                    error.Data.Add("extension", Path.GetExtension(filePath));
                    throw error;
            }
        }
    }
}