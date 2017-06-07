namespace Wikiled.Text.Analysis.POS.Tags
{
    public class SentenceFinalPunctuation : BasePOSType
    {
        private SentenceFinalPunctuation()
        {
        }

        public static SentenceFinalPunctuation Instance { get; } = new SentenceFinalPunctuation();

        public override string Description => "DeclarativeSentence-final punctuation";

        public override string Tag => ".";

        public override WordType WordType => WordType.Symbol;
    }
}
