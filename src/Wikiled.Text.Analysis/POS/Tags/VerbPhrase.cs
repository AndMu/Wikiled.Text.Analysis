namespace Wikiled.Text.Analysis.POS.Tags
{
    public class VerbPhrase : BasePOSType
    {
        private readonly static VerbPhrase instance = new VerbPhrase();

        private VerbPhrase()
        {
        }

        public override string Description
        {
            get { return "Verb Phrase"; }
        }

        public override WordType WordType
        {
            get { return WordType.Verb; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static VerbPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "VP"; }
        }
    }
}

