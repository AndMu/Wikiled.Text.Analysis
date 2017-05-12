namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PossessiveEnding : BasePOSType
    {
        private readonly static PossessiveEnding instance = new PossessiveEnding();

        private PossessiveEnding()
        {
        }

        public override string Description
        {
            get { return "Possessive ending"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public static PossessiveEnding Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "POS"; }
        }
    }
}
