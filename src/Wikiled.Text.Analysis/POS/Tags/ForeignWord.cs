namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ForeignWord : BasePOSType
    {
        private ForeignWord()
        {
        }

        public override string Description => "Foreign word";

        public override WordType WordType => WordType.Unknown;

        public static ForeignWord Instance { get; } = new ForeignWord();

        public override string Tag => "FW";
    }
}

