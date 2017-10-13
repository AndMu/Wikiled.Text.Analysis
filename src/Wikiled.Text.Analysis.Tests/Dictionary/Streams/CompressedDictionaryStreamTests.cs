using NUnit.Framework;
using System.Linq;
using Wikiled.Text.Analysis.Dictionary.Streams;

namespace Wikiled.Text.Analysis.Tests.Dictionary.Streams
{
    [TestFixture]
    public class CompressedDictionaryStreamTests
    {
        [Test]
        public void Construct()
        {
            var stream = new CompressedDictionaryStream("Resources.Dictionary.RawEnglish.dat", new InternalStreamSource());
            var table = stream.ReadDataFromStream(double.Parse).ToDictionary(item => item.Word, item => item.Value);
            Assert.AreEqual(44323, table.Count);
        }
    }
}
