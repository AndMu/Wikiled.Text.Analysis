namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Predeterminer : BasePOSType
    {
        private readonly static Predeterminer instance = new Predeterminer();

        private Predeterminer()
        {
        }

        public override string Description
        {
            get { return "Predeterminer"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public static Predeterminer Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "PDT"; }
        }
    }
}
