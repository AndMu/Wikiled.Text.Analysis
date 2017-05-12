namespace Wikiled.Text.Analysis.POS.Tags
{
    public class QuestionPhrase : BasePOSType
    {
        private QuestionPhrase()
        {
        }

        public override string Description => "Question Phrase";

        public override WordType WordType => WordType.Unknown;

        public override bool IsGroup => true;

        public static QuestionPhrase Instance { get; } = new QuestionPhrase();

        public override string Tag => "QP";
    }
}
