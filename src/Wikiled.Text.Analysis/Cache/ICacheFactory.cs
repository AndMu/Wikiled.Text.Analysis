using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Cache
{
    public interface ICacheFactory
    {
        ICachedDocumentsSource Create(POSTaggerType tagger);
    }
}