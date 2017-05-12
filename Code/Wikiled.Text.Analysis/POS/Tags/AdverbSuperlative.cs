namespace Wikiled.Text.Analysis.POS.Tags
{
    public class AdverbSuperlative : BasePOSType
    {
        private readonly static AdverbSuperlative instance = new AdverbSuperlative();

        private AdverbSuperlative()
        {
        }

        public override string Description
        {
            get { return "Adverb, superlative"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adverb; }
        }

        public static AdverbSuperlative Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "RBS"; }
        }
    }
}

