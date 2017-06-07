namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WhAdjectival : BasePOSType
    {
        private WhAdjectival()
        {
        }

        public override string Description => "wh-adjectival phrase,e .g. how cold";

        public override WordType WordType => WordType.Adjective;

        public override bool IsGroup => true;

        public static WhAdjectival Instance { get; } = new WhAdjectival();

        public override string Tag => "WHADJP";
    }
}
