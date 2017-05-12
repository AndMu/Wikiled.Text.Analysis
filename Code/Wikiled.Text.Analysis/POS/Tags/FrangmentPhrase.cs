namespace Wikiled.Text.Analysis.POS.Tags
{
    public class FrangmentPhrase : BasePOSType
    {
        private FrangmentPhrase()
        {
        }

        public override string Description => "Fragment Phrase";

        public override WordType WordType => WordType.Unknown;

        public override bool IsGroup => true;

        public static FrangmentPhrase Instance { get; } = new FrangmentPhrase();

        public override string Tag => "FRAG";
    }
}
