namespace Wikiled.Text.Analysis.POS.Tags
{
    public class To : BasePOSType
    {
        private To()
        {

        }

        public override string Description => "To";

        public override WordType WordType => WordType.Unknown;

        public static To Instance { get; } = new To();

        public override string Tag => "TO";
    }
}
