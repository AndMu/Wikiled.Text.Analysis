namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Adverb : BasePOSType
    {
        private readonly static Adverb instance = new Adverb();

        private Adverb()
        {
        }

        public override string Description
        {
            get { return "Adverb"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adverb; }
        }

        public static Adverb Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "RB"; }
        }
    }
}

