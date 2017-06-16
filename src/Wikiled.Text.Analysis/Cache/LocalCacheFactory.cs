using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Cache
{
    public class LocalCacheFactory : ICacheFactory
    {
        public ICachedDocumentsSource Create(POSTaggerType tagger)
        {
            return new LocalDocumentsCache();
        }
    }
}
