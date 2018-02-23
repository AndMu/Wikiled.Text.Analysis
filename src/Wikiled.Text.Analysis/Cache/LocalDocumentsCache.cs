using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using Wikiled.Common.Arguments;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalDocumentsCache : ICachedDocumentsSource
    {
        private readonly ConcurrentDictionary<string, Document> documentTable = new ConcurrentDictionary<string, Document>(StringComparer.OrdinalIgnoreCase);

        public Task<Document> GetById(string id)
        {
            Guard.NotNullOrEmpty(() => id, id);
            Document document;
            documentTable.TryGetValue(id, out document);
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
            documentTable[document.Id] = document;
            return Task.FromResult(true);
        }
    }
}
