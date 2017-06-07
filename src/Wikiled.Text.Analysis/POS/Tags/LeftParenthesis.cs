namespace Wikiled.Text.Analysis.POS.Tags
{
    public class LeftParenthesis : BasePOSType
    {
        private readonly static LeftParenthesis instance = new LeftParenthesis();

        private LeftParenthesis()
        {
        }

        public override string Description
        {
            get { return "Left parenthesi"; }
        }

        public override WordType WordType
        {
            get { return WordType.SeparationSymbol; }
        }

        public static LeftParenthesis Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "-LRB-"; }
        }
    }
}

