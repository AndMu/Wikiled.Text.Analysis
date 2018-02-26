using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.Tests.Dictionary.Streams
{
    [TestFixture]
    public class DictionaryStreamTests
    {
        [Test]
        public void Construct()
        {
            var path = ConfigurationManager.AppSettings["resources"];
            var file = Path.Combine(TestContext.CurrentContext.TestDirectory, path, @"Embedded\Dictionary\NRC.txt");
            var stream = new DictionaryStream(file, new FileStreamSource());
            var table = stream.ReadDataFromStream(double.Parse).ToArray();
            Assert.AreEqual(141820, table.Length);
            file = Path.Combine(TestContext.CurrentContext.TestDirectory, "NRC.dat");
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var outStream = new CompressedDictionaryStream(file, new FileStreamSource());
            DictionaryStreamExtension.WriteStream(file, table.Select(item => new KeyValuePair<string, double>(item.Word, item.Value)), Encoding.ASCII);
            table = outStream.ReadDataFromStream(double.Parse).ToArray();
            Assert.AreEqual(141820, table.Length);
        }
    }
}
