namespace Wikiled.Text.Analysis.POS.Tags
{
    public class DelarativeSentence : BasePOSType
    {
        private DelarativeSentence()
        {
        }

        public override string Description => "Delarative Sentence";

        public override WordType WordType => WordType.Sentence;

        public override bool IsGroup => true;

        public static DelarativeSentence Instance { get; } = new DelarativeSentence();

        public override string Tag => "S";
    }
}
