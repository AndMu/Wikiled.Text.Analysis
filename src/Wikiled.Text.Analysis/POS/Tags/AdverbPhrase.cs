namespace Wikiled.Text.Analysis.POS.Tags
{
    public class AdverbPhrase : BasePOSType
    {
        private readonly static AdverbPhrase instance = new AdverbPhrase();

        private AdverbPhrase()
        {
        }

        public override string Description
        {
            get { return "Adverb Phrase"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adverb; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static AdverbPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "ADVP"; }
        }
    }
}

