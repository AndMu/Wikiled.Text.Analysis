namespace Wikiled.Text.Analysis.POS.Tags
{
    public class LeftOpenDoubleQuote : BasePOSType
    {
        private readonly static LeftOpenDoubleQuote instance = new LeftOpenDoubleQuote();

        private LeftOpenDoubleQuote()
        {
        }

        public override string Description
        {
            get { return "Left open double quote"; }
        }

        public override WordType WordType
        {
            get { return WordType.SeparationSymbol; }
        }

        public static LeftOpenDoubleQuote Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "``"; }
        }
    }
}
