using Microsoft.Extensions.Caching.Memory;
using Wikiled.Common.Arguments;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalCacheFactory : ICacheFactory
    {
        private readonly IMemoryCache cache;

        public LocalCacheFactory(IMemoryCache cache)
        {
            Guard.NotNull(() => cache, cache);
            this.cache = cache;
        }

        public ICachedDocumentsSource Create(POSTaggerType tagger)
        {
            return new LocalDocumentsCache(cache);
        }
    }
}
