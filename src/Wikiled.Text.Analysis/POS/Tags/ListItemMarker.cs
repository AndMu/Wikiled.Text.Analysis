namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ListItemMarker : BasePOSType
    {
        private readonly static ListItemMarker instance = new ListItemMarker();

        private ListItemMarker()
        {

        }

        public override string Description
        {
            get { return "List item marker"; }
        }

        public override WordType WordType
        {
            get { return WordType.SeparationSymbol; }
        }

        public static ListItemMarker Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "LS"; }
        }
    }
}
