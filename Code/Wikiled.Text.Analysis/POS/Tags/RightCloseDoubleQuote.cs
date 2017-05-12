namespace Wikiled.Text.Analysis.POS.Tags
{
    public class RightCloseDoubleQuote : BasePOSType
    {
        private readonly static RightCloseDoubleQuote instance = new RightCloseDoubleQuote();

        private RightCloseDoubleQuote()
        {
        }

        public override string Description
        {
            get { return "Right close double quote"; }
        }

        public override WordType WordType
        {
            get { return WordType.SeparationSymbol; }
        }

        public static RightCloseDoubleQuote Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "''"; }
        }
    }
}

