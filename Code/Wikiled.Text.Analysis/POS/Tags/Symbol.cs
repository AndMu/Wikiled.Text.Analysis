namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Symbol : BasePOSType
    {
        private Symbol()
        {

        }

        public override string Description => "Symbol";

        public override WordType WordType => WordType.Symbol;

        public static Symbol Instance { get; } = new Symbol();

        public override string Tag => "SYM";
    }
}
