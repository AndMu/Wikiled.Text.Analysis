using System.Threading.Tasks;
using Wikiled.Text.Analysis.Structure;

namespace Wikiled.Text.Analysis.Cache
{
    public interface ICachedDocumentsSource
    {
        Task<Document> GetById(string id);

        Task<Document> GetCached(Document original);

        Task<Document> GetCached(string text);

        Task<bool> Save(Document document);
    }
}