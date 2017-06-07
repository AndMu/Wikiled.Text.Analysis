namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Determiner : BasePOSType
    {
        private Determiner()
        {
        }

        public override string Description => "Determiner";

        public override WordType WordType => WordType.Unknown;

        public static Determiner Instance { get; } = new Determiner();

        public override string Tag => "DT";
    }
}
