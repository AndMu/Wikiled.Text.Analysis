using Jitbit.Utils;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using Wikiled.Common.Utilities.Helpers;
using Wikiled.Text.Analysis.Extensions;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalDocumentsCache : ICachedDocumentsSource
    {
        private readonly FastCache<string, LightDocument> cache = new();

        private readonly ILogger<LocalDocumentsCache> log;

        public LocalDocumentsCache(ILogger<LocalDocumentsCache> log)
        {
            this.cache = cache ?? throw new ArgumentNullException(nameof(cache));
            this.log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public Task<LightDocument> GetCached(IDocument original)
        {
            if (original == null)
            {
                throw new ArgumentNullException(nameof(original));
            }

            if (cache.TryGet(original.GetId(), out LightDocument document))
            {
                log.LogDebug("Found in cache using document id: {0}", document.Id);
                return Task.FromResult(document);
            }

            if (cache.TryGet(original.GetTextId(), out document))
            {
                log.LogDebug("Found in cache using text - document id: {0}", document.Id);
            }

            return Task.FromResult(document);
        }

        public Task<bool> Save(LightDocument document)
        {
            document = document.CloneJson();
            // Save data in cache.
            cache.TryAdd(document.GetId(), document, TimeSpan.FromMinutes(1));
            cache.TryAdd(document.GetTextId(), document, TimeSpan.FromMinutes(1));
            return Task.FromResult(true);
        }
    }
}
