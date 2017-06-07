namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WhAdverb : BasePOSType
    {
        private readonly static WhAdverb instance = new WhAdverb();

        private WhAdverb()
        {
        }

        public override string Description
        {
            get { return "wh-adverb"; }
        }

        public override WordType WordType
        {
            get { return WordType.Adverb; }
        }

        public static WhAdverb Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "WRB"; }
        }
    }
}
