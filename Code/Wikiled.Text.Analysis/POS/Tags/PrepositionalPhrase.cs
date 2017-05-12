namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PrepositionalPhrase : BasePOSType
    {
        private readonly static PrepositionalPhrase instance = new PrepositionalPhrase();

        private PrepositionalPhrase()
        {
        }

        public override string Description
        {
            get { return "Prepositional Phrase"; }
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

        public static PrepositionalPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "PP"; }
        }
    }
}

