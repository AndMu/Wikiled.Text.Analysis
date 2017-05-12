namespace Wikiled.Text.Analysis.POS.Tags
{
    public class InterjectionPhrase : BasePOSType
    {
        private readonly static InterjectionPhrase instance = new InterjectionPhrase();

        private InterjectionPhrase()
        {
        }

        public override string Description
        {
            get { return "Interjection"; }
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

        public static InterjectionPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "INTJ"; }
        }
    }
}


