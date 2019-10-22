using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;
using Wikiled.Text.Analysis.Structure.Light;

namespace Wikiled.Text.Analysis.Cache
{
    public interface ICachedDocumentsSource
    {
        Task<LightDocument> GetCached(LightDocument original);

        Task<bool> Save(LightDocument document);
    }
}