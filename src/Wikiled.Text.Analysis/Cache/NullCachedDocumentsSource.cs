using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Cache
{
    public class NullCachedDocumentsSource : ICachedDocumentsSource
    {
        public Task<LightDocument> GetCached(IDocument original)
        {
            return Task.FromResult((LightDocument)default);
        }

        public Task<bool> Save(LightDocument annotation)
        {
            return Task.FromResult(false);
        }
    }
}
