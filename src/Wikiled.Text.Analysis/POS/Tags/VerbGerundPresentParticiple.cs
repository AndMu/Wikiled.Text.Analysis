namespace Wikiled.Text.Analysis.POS.Tags
{
    public class VerbGerundPresentParticiple : BasePOSType
    {
        private VerbGerundPresentParticiple()
        {
        }

        public override string Description => "Verb, gerund/present participle";

        public override WordType WordType => WordType.Verb;

        public static VerbGerundPresentParticiple Instance { get; } = new VerbGerundPresentParticiple();

        public override string Tag => "VBG";
    }
}


