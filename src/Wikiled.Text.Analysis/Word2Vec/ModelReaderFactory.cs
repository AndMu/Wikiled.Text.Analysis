using System;
using System.IO;
using System.IO.Compression;
using Microsoft.Extensions.Logging;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public class ModelReaderFactory : IModelReaderFactory
    {
        private readonly ILoggerFactory loggerFactory;

        public ModelReaderFactory(ILoggerFactory loggerFactory)
        {
            this.loggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public IWordModel Construct(string filePath)
        {
            using (var fileStream = OpenStream(filePath))
            {
                var ext = Path.GetExtension(filePath);
                if (ext == ".gz")
                {
                    ext = Path.GetExtension(Path.GetFileNameWithoutExtension(filePath));
                }

                var reader = GetReader(fileStream, ext.ToLower());
                return reader.Open();
            }
        }

        private Stream OpenStream(string filePath)
        {
            var fileStream = File.OpenRead(filePath);
            if (Path.GetExtension(filePath).ToLower() == ".gz")
            {
                return new GZipStream(fileStream, CompressionMode.Decompress);
            }

            return fileStream;
        }

        private IModelReader GetReader(Stream stream, string fileExtension)
        {
            switch (fileExtension)
            {
                case ".txt":
                    return new TextModelReader(loggerFactory, stream);
                case ".bin":
                    return new BinaryModelReader(loggerFactory, stream);
                default:
                    var error = new InvalidOperationException("Unrecognized file type");
                    error.Data.Add("extension", fileExtension);
                    throw error;
            }
        }

    }
}