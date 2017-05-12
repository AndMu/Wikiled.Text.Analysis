namespace Wikiled.Text.Analysis.POS.Tags
{
    public class XSPhrase : BasePOSType
    {
        private static readonly XSPhrase instance = new XSPhrase();

        private XSPhrase()
        {
        }

        public override string Description
        {
            get { return "XS"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public override bool IsGroup
        {
            get { return true; }
        }

        public static XSPhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "XS"; }
        }
    }
}