namespace Wikiled.Text.Analysis.POS.Tags
{
    public class NounProper : BasePOSType
    {
        private readonly static NounProper instance = new NounProper();

        private NounProper()
        {
        }

        public override string Description
        {
            get { return "Proper noun, singular"; }
        }

        public override WordType WordType
        {
            get { return WordType.Noun; }
        }

        public static NounProper Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "NNP"; }
        }
    }
}

