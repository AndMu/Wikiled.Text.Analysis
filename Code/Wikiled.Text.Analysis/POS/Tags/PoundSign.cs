namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PoundSign : BasePOSType
    {
        private PoundSign()
        {
        }

        public static PoundSign Instance { get; } = new PoundSign();

        public override string Description => "Pound sign";

        public override string Tag => "#";

        public override WordType WordType => WordType.Symbol;
    }
}
