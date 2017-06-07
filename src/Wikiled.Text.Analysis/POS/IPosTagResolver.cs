using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.POS
{
    public interface IPosTagResolver
    {
        BasePOSType GetPOS(string word);
    }
}
