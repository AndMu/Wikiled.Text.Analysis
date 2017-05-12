namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Comma : BasePOSType
    {
        private Comma()
        {
        }

        public override string Description => "Comma";

        public override WordType WordType => WordType.Conjunction;

        public static Comma Instance { get; } = new Comma();

        public override string Tag => ",";
    }
}

