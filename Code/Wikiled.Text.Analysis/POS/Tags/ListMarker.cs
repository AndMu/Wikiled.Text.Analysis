namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ListMarker : BasePOSType
    {
        private readonly static ListMarker instance = new ListMarker();

        private ListMarker()
        {
        }

        public override string Description
        {
            get { return "List marker"; }
        }

        public override WordType WordType
        {
            get { return WordType.SeparationSymbol; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static ListMarker Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "LST"; }
        }
    }
}


