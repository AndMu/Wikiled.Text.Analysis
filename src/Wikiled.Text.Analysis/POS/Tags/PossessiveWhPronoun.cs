namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PossessiveWhPronoun : BasePOSType
    {
        private readonly static PossessiveWhPronoun instance = new PossessiveWhPronoun();

        private PossessiveWhPronoun()
        {
        }

        public override string Description
        {
            get { return "Possessive wh-pronoun"; }
        }

        public override WordType WordType
        {
            get { return WordType.Pronoun; }
        }

        public static PossessiveWhPronoun Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "WP$"; }
        }
    }
}

