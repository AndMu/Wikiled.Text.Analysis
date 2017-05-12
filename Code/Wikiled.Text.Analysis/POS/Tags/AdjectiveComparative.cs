namespace Wikiled.Text.Analysis.POS.Tags
{
    public class AdjectiveComparative : BasePOSType
    {
        private readonly static AdjectiveComparative instance = new AdjectiveComparative();

        private AdjectiveComparative()
        {
        }

        public override string Description
        {
            get { return "Adjective, comparative"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adjective; }
        }

        public static AdjectiveComparative Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "JJR"; }
        }
    }
}


