using Wikiled.Text.Analysis.POS.Tags;

namespace Wikiled.Text.Analysis.POS
{
    public interface IPOSTagger
    {
        BasePOSType GetTag(string word);
    }
}