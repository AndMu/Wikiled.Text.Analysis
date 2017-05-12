namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WhPronoun : BasePOSType
    {
        private readonly static WhPronoun instance = new WhPronoun();

        private WhPronoun()
        {
        }

        public override string Description
        {
            get { return  "wh-pronoun"; }
        }

        public override WordType WordType
        {
            get { return WordType.Pronoun; }
        }

        public static WhPronoun Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "WP"; }
        }
    }
}
