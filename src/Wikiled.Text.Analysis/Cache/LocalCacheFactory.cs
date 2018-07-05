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
            this.cache = cache ?? throw new System.ArgumentNullException(nameof(cache));
        }

        public ICachedDocumentsSource Create(POSTaggerType tagger)
        {
            return new LocalDocumentsCache(cache);
        }
    }
}
