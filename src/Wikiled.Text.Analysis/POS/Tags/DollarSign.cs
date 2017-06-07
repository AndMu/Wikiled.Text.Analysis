namespace Wikiled.Text.Analysis.POS.Tags
{
    public class DollarSign : BasePOSType
    {
        private DollarSign()
        {
        }

        public override string Description => "Dollar sign";

        public override WordType WordType => WordType.Symbol;

        public static DollarSign Instance { get; } = new DollarSign();

        public override string Tag => "$";
    }
}

