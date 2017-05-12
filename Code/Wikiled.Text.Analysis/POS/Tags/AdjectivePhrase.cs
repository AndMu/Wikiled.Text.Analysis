namespace Wikiled.Text.Analysis.POS.Tags
{
    public class AdjectivePhrase : BasePOSType
    {
        private readonly static AdjectivePhrase instance = new AdjectivePhrase();

        private AdjectivePhrase()
        {
        }

        public override string Description
        {
            get { return "Adjective Phrase"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adjective; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static AdjectivePhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "ADJP"; }
        }
    }
}

