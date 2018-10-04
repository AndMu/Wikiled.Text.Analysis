using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NUnit.Framework;
using Wikiled.Text.Analysis.Cache;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Tests.Cache
{
    [TestFixture]
    public class LocalDocumentsCacheTests
    {
        private LocalDocumentsCache instance;

        [SetUp]
        public void Setup()
        {
            instance = new LocalDocumentsCache(new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public async Task Test()
        {
            Assert.Throws<ArgumentNullException>(() => instance.GetCached(null));
            Document doc = new Document();
            doc.Id = "Test";
            doc.Text = "Test";
            var result = await instance.GetCached(doc).ConfigureAwait(false);
            Assert.IsNull(result);
            
            await instance.Save(doc).ConfigureAwait(false);
            result = await instance.GetCached(doc).ConfigureAwait(false);
            Assert.AreNotSame(doc, result);
            Assert.AreEqual("Test", result.Text);
            doc.Id = "2";
            result = await instance.GetCached(doc).ConfigureAwait(false);
            Assert.AreNotSame(doc, result);
            Assert.AreEqual("Test", result.Text);
        }
    }
}
