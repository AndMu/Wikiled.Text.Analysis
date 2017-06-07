namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Parenthetical : BasePOSType
    {
        private Parenthetical()
        {
        }

        public override string Description => "Parenthetical Phrase";

        public override WordType WordType => WordType.Unknown;

        public override bool IsGroup => true;

        public static Parenthetical Instance { get; } = new Parenthetical();

        public override string Tag => "PRN";
    }
}
