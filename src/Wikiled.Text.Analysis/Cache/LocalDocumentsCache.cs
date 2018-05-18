using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;
using Wikiled.Common.Arguments;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalDocumentsCache : ICachedDocumentsSource
    {
        private readonly IMemoryCache cache;

        public LocalDocumentsCache(IMemoryCache cache)
        {
            Guard.NotNull(() => cache, cache);
            this.cache = cache;
        }

        public Task<Document> GetById(string id)
        {
            Guard.NotNullOrEmpty(() => id, id);
            cache.TryGetValue(id, out Document document);
            return Task.FromResult(document);
        }

        public Task<Document> GetCached(Document original)
        {
            Guard.NotNull(() => original, original);
            return GetById(original.Id);
        }

        public Task<Document> GetCached(string text)
        {
            return Task.FromResult((Document)null);
        }

        public Task<bool> Save(Document document)
        {
            Guard.NotNull(() => document, document);
            var cacheEntryOptions = new MemoryCacheEntryOptions()
                .SetSlidingExpiration(TimeSpan.FromMinutes(1));

            // Save data in cache.
            cache.Set(document.Id, document, cacheEntryOptions);
            return Task.FromResult(true);
        }
    }
}
