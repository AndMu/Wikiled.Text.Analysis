namespace Wikiled.Text.Analysis.POS.Tags
{
    public class WHNounPhrase : BasePOSType
    {
        private WHNounPhrase()
        {
        }

        public override string Description => "WH-Noun Phrase";

        public override WordType WordType => WordType.Noun;

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static WHNounPhrase Instance { get; } = new WHNounPhrase();

        public override string Tag => "WHNP";
    }
}

