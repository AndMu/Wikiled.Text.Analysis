namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Colon : BasePOSType
    {
        private readonly static Colon instance = new Colon();

        private Colon()
        {
        }

        public override string Description
        {
            get { return "Colon, semi-colon"; }
        }

        public override WordType WordType
        {
            get { return WordType.Conjunction; }
        }

        public static Colon Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return ":"; }
        }
    }
}

