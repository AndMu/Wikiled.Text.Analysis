using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public class NullCachedDocumentsSource : ICachedDocumentsSource
    {
        public static readonly NullCachedDocumentsSource Instance = new NullCachedDocumentsSource();

        private NullCachedDocumentsSource()
        {
        }

        public Task<Document> GetById(string id)
        {
            return Task.FromResult((Document)null);
        }

        public Task<Document> GetCached(Document original)
        {
            return Task.FromResult((Document)null);
        }

        public Task<Document> GetCached(string text)
        {
            return Task.FromResult((Document)null);
        }

        public Task<bool> Save(Document annotation)
        {
            return Task.FromResult(false);
        }
    }
}
