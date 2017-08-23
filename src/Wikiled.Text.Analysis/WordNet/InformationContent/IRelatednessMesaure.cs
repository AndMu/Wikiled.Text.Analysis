using Wikiled.Text.Analysis.POS;
using Wikiled.Text.Analysis.WordNet.Engine;

namespace Wikiled.Text.Analysis.WordNet.InformationContent
{
    public interface IRelatednessMesaure
    {
        double Measure(SynSet synSet1, SynSet synSet2);

        double Measure(string word1, string word2, WordType type = WordType.Noun);
    }
}