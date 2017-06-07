namespace Wikiled.Text.Analysis.POS.Tags
{
    public class AdjectiveSuperlative : BasePOSType
    {
        private readonly static AdjectiveSuperlative instance = new AdjectiveSuperlative();

        private AdjectiveSuperlative()
        {
        }

        public override string Description
        {
            get { return "Adjective, superlative"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adjective; }
        }

        public static AdjectiveSuperlative Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "JJS"; }
        }
    }
}


