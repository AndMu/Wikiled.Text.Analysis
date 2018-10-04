using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using NLog;
using Wikiled.Common.Utilities.Helpers;
using Wikiled.Text.Analysis.Extensions;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalDocumentsCache : ICachedDocumentsSource
    {
        private readonly IMemoryCache cache;

        private static readonly Logger log = LogManager.GetCurrentClassLogger();

        public LocalDocumentsCache(IMemoryCache cache)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
        }

        public Task<Document> GetCached(Document original)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (cache.TryGetValue(GetId(original), out Document document))
            {
                log.Debug("Found in cache using document id: {0}", document.Id);
                return Task.FromResult(document);
            }

            if (cache.TryGetValue(GetTextId(original), out document))
            {
                log.Debug("Found in cache using text - document id: {0}", document.Id);
            }

            return Task.FromResult(document);
        }

        public Task<bool> Save(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            document = document.CloneJson();
            // Save data in cache.
            cache.Set(GetId(document), document, cacheEntryOptions);
            cache.Set(GetTextId(document), document, cacheEntryOptions);
            return Task.FromResult(true);
        }

        private string GetTextId(Document document)
        {
            return $"Text:{document.Text.GenerateKey()}";
        }

        private string GetId(Document document)
        {
            return $"Document:{document.Id}:{document.Text.GenerateKey()}";
        }
    }
}
