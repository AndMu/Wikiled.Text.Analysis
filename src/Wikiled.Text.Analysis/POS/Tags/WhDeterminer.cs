namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WhDeterminer : BasePOSType
    {
        private readonly static WhDeterminer instance = new WhDeterminer();

        private WhDeterminer()
        {

        }

        public override string Description
        {
            get { return "wh-determiner"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public static WhDeterminer Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "WDT"; }
        }
    }
}
