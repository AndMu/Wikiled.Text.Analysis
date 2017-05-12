namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PhraseUnknown : BasePOSType
    {
        public override string Description => "Root";

        public override string Tag => "X";

        public override bool IsGroup => true;

        public override WordType WordType => WordType.Unknown;

        public static PhraseUnknown Instance { get; } = new PhraseUnknown();
    }
}
