using Wikiled.Text.Analysis.WordNet.Engine;

namespace Wikiled.Text.Analysis.WordNet.InformationContent
{
    public interface IInformationContentResnik
    {
        double GetIC(SynSet synSet);

        double GetFrequency(SynSet synSet);

        double TotalNouns { get; }

        double TotalVerbs { get; }
    }
}