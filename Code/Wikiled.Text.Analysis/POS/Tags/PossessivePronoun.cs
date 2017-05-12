namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PossessivePronoun : BasePOSType
    {
        private readonly static PossessivePronoun instance = new PossessivePronoun();

        private PossessivePronoun()
        {
        }

        public override string Description
        {
            get { return "Possessive pronoun"; }
        }

        public override WordType WordType
        {
            get { return WordType.Pronoun; }
        }

        public static PossessivePronoun Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "PRP$"; }
        }
    }
}

