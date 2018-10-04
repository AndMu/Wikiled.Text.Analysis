using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public class NullCachedDocumentsSource : ICachedDocumentsSource
    {
        public Task<Document> GetCached(Document original)
        {
            return Task.FromResult((Document)null);
        }

        public Task<bool> Save(Document annotation)
        {
            return Task.FromResult(false);
        }
    }
}
