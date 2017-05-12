namespace Wikiled.Text.Analysis.POS.Tags
{
    public class NounPlural : BasePOSType
    {
        private NounPlural()
        {
        }

        public override string Description => "Noun, plural";

        public override WordType WordType => WordType.Noun;

        public static NounPlural Instance { get; } = new NounPlural();

        public override string Tag => "NNS";
    }
}

