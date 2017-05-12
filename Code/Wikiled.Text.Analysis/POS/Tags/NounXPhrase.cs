namespace Wikiled.Text.Analysis.POS.Tags
{
    public class NounXPhrase : BasePOSType
    {
        private readonly static NounXPhrase instance = new NounXPhrase();

        private NounXPhrase()
        {
        }

        public override string Description
        {
            get { return "NX Phrase"; }
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

        public static NounXPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "NX"; }
        }
    }
}
