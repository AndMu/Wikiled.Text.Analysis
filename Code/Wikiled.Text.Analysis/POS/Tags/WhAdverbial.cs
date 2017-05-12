namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WhAdverbial : BasePOSType
    {
        private WhAdverbial()
        {
        }

        public override string Description => "Wh-adverbial phrase, e.g. why";

        public override WordType WordType => WordType.Adverb;

        public override bool IsGroup => true;

        public static WhAdverbial Instance { get; } = new WhAdverbial();

        public override string Tag => "WHADVP";
    }
}
