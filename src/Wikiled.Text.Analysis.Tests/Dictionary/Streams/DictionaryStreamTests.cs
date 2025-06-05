using NUnit.Framework;
using NUnit.Framework.Legacy;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.Tests.Dictionary.Streams
{
    [TestFixture]
    public class DictionaryStreamTests
    {
        [Test]
        public void Construct()
        {
            var path = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly()!.FullName!)!);
            var file = Path.Combine(path, "Resources", "Embedded", "Dictionary", "NRC.txt");
            var stream = new DictionaryStream(file, new FileStreamSource());
            var table = stream.ReadDataFromStream(double.Parse).ToArray();
            ClassicAssert.AreEqual(141820, table.Length);
            file = Path.Combine(TestContext.CurrentContext.TestDirectory, "NRC.dat");
            if (File.Exists(file))
            {
                File.Delete(file);
            }

            var outStream = new CompressedDictionaryStream(file, new FileStreamSource());
            DictionaryStreamExtension.WriteStream(file, table.Select(item => new KeyValuePair<string, double>(item.Word, item.Value)), Encoding.ASCII);
            table = outStream.ReadDataFromStream(double.Parse).ToArray();
            ClassicAssert.AreEqual(141820, table.Length);
        }
    }
}
