namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ConjunctionPhrase : BasePOSType
    {
        private readonly static ConjunctionPhrase instance = new ConjunctionPhrase();

        private ConjunctionPhrase()
        {
        }

        public override string Description
        {
            get { return "Conjunction Phrase"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static ConjunctionPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "CONJP"; }
        }
    }
}

