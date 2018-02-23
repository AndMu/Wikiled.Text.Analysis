using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Wikiled.Common.Helpers;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public static class DictionaryStreamExtension
    {
        public static IEnumerable<(string Word, T Value)> ReadDataFromStream<T>(this IDictionaryStream streamSource, Func<string, T> coverver, char separator = '\t')
        {
            using (var reader = streamSource.ConstructReadStream())
            {
                string CoverterText(string data) => data;
                ReadTabResourceDataFile boosterData = new ReadTabResourceDataFile(streamSource.Name, reader);
                boosterData.UseDefaultIfNotFound = true;
                boosterData.Separator = separator;
                foreach (var item in boosterData.ReadData(CoverterText, coverver))
                {
                    yield return item;
                }
            }
        }

        public static void WriteStream(string name, IEnumerable<KeyValuePair<string, double>> data, Encoding encoding, char separator = '\t')
        {
            using (var file = new FileStream(name, FileMode.Create))
            {
                StringBuilder builder = new StringBuilder();
                foreach (var record in data)
                {
                    builder.AppendLine($"{record.Key}{separator}{record.Value}");
                }

                var bytes = builder.ToString().Zip();
                file.Write(bytes, 0, bytes.Length);
                file.Flush();
            }
        }
    }
}
