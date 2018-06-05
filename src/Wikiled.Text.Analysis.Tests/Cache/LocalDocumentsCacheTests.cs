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
            Assert.Throws<ArgumentNullException>(() => instance.GetById(null));
            Assert.Throws<ArgumentNullException>(() => instance.GetCached((Document)null));
            var result = await instance.GetById("Test").ConfigureAwait(false);
            Assert.IsNull(result);
            Document doc = new Document();
            doc.Id = "Test";
            await instance.Save(doc).ConfigureAwait(false);
            result = await instance.GetById("Test").ConfigureAwait(false);
            Assert.AreSame(doc, result);
            result = await instance.GetCached(doc).ConfigureAwait(false);
            Assert.AreSame(doc, result);
        }
    }
}
