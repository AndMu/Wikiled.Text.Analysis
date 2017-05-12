namespace Wikiled.Text.Analysis.POS.Tags
{
    public class NounProperPlural : BasePOSType
    {
        private NounProperPlural()
        {
        }

        public override string Description => "Proper noun, plural";

        public override WordType WordType => WordType.Noun;

        public static NounProperPlural Instance { get; } = new NounProperPlural();

        public override string Tag => "NNPS";
    }
}
