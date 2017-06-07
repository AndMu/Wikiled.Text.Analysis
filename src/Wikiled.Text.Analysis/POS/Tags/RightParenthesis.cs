namespace Wikiled.Text.Analysis.POS.Tags
{
    public class RightParenthesis : BasePOSType
    {
        private readonly static RightParenthesis instance = new RightParenthesis();

        private RightParenthesis()
        {
        }

        public override string Description
        {
            get { return "Right parenthesis"; }
        }

        public override WordType WordType
        {
            get { return WordType.SeparationSymbol; }
        }

        public static RightParenthesis Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "-RRB-"; }
        }
    }
}

