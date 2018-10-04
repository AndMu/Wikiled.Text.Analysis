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

            if (cache.TryGetValue(original.GetId(), out Document document))
            {
                log.Debug("Found in cache using document id: {0}", document.Id);
                return Task.FromResult(document);
            }

            if (cache.TryGetValue(original.GetTextId(), out document))
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
            cache.Set(document.GetId(), document, cacheEntryOptions);
            cache.Set(document.GetTextId(), document, cacheEntryOptions);
            return Task.FromResult(true);
        }
    }
}
