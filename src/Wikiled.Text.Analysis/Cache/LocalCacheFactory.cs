using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalCacheFactory : ICacheFactory
    {
        private readonly ILogger<LocalDocumentsCache> log;

        public LocalCacheFactory(ILogger<LocalDocumentsCache> log)
        {
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public ICachedDocumentsSource Create(POSTaggerType tagger)
        {
            return new LocalDocumentsCache(log);
        }
    }
}
