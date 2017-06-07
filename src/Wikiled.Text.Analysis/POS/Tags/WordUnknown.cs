namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WordUnknown : BasePOSType
    {
        public override string Description => "Root";

        public override string Tag => "XX";

        public override WordType WordType => WordType.Unknown;

        public static WordUnknown Instance { get; } = new WordUnknown();
    }
}
