using Wikiled.Text.Analysis.POS;

namespace Wikiled.Text.Analysis.Cache
{
    public class NullCacheFactory : ICacheFactory
    {
        public ICachedDocumentsSource Create(POSTaggerType tagger)
        {
            return new NullCachedDocumentsSource();
        }
    }
}
