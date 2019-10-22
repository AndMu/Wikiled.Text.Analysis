using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wikiled.Common.Utilities.Helpers;
using Wikiled.Text.Analysis.Extensions;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalDocumentsCache : ICachedDocumentsSource
    {
        private readonly IMemoryCache cache;

        private readonly ILogger<LocalDocumentsCache> log;

        public LocalDocumentsCache(ILogger<LocalDocumentsCache> log, IMemoryCache cache)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task<LightDocument> GetCached(LightDocument original)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (cache.TryGetValue(original.GetId(), out LightDocument document))
            {
                log.LogDebug("Found in cache using document id: {0}", document.Id);
                return Task.FromResult(document);
            }

            if (cache.TryGetValue(original.GetTextId(), out document))
            {
                log.LogDebug("Found in cache using text - document id: {0}", document.Id);
            }

            return Task.FromResult(document);
        }

        public Task<bool> Save(LightDocument document)
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
