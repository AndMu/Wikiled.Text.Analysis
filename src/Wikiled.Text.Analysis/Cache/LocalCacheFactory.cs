using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalCacheFactory : ICacheFactory
    {
        private readonly IMemoryCache cache;

        private readonly ILogger<LocalDocumentsCache> log;

        public LocalCacheFactory(ILogger<LocalDocumentsCache> log, IMemoryCache cache)
        {
            this.cache = cache ?? throw new System.ArgumentNullException(nameof(cache));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public ICachedDocumentsSource Create(POSTaggerType tagger)
        {
            return new LocalDocumentsCache(log, cache);
        }
    }
}
