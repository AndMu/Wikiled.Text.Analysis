namespace Wikiled.Text.Analysis.POS.Tags
{
    public class NounPhrase : BasePOSType
    {
        private readonly static NounPhrase instance = new NounPhrase();

        private NounPhrase()
        {
        }

        public override string Description
        {
            get { return "Noun Phrase"; }
        }

        public override WordType WordType
        {
            get { return WordType.Noun; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static NounPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "NP"; }
        }
    }
}

