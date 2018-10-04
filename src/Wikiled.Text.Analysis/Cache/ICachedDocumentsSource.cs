using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public interface ICachedDocumentsSource
    {
        Task<Document> GetCached(Document original);

        Task<bool> Save(Document document);
    }
}