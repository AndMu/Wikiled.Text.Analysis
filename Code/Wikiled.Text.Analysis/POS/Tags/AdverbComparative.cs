namespace Wikiled.Text.Analysis.POS.Tags
{
    public class AdverbComparative : BasePOSType
    {
        private readonly static AdverbComparative instance = new AdverbComparative();

        private AdverbComparative()
        {
        }

        public override string Description
        {
            get { return "Adverb, comparative"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adverb; }
        }

        public static AdverbComparative Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "RBR"; }
        }
    }
}

