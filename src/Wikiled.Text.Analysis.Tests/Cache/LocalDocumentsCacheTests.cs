using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging.Abstractions;
using NUnit.Framework;
using NUnit.Framework.Legacy;
using Wikiled.Text.Analysis.Cache;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Tests.Cache
{
    [TestFixture]
    public class LocalDocumentsCacheTests
    {
        private LocalDocumentsCache instance;

        [SetUp]
        public void Setup()
        {
            instance = new LocalDocumentsCache(new NullLogger<LocalDocumentsCache>(), new MemoryCache(new MemoryCacheOptions()));
        }

        [Test]
        public async Task Test()
        {
            ClassicAssert.Throws<ArgumentNullException>(() => instance.GetCached(null));
            var doc = new LightDocument();
            doc.Id = "Test";
            doc.Text = "Test";
            var result = await instance.GetCached(doc).ConfigureAwait(false);
            ClassicAssert.IsNull(result);
            
            await instance.Save(doc).ConfigureAwait(false);
            result = await instance.GetCached(doc).ConfigureAwait(false);
            ClassicAssert.AreNotSame(doc, result);
            ClassicAssert.AreEqual("Test", result.Text);
            doc.Id = "2";
            result = await instance.GetCached(doc).ConfigureAwait(false);
            ClassicAssert.AreNotSame(doc, result);
            ClassicAssert.AreEqual("Test", result.Text);
        }
    }
}
