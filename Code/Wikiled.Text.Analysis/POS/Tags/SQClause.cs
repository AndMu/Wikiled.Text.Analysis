namespace Wikiled.Text.Analysis.POS.Tags
{
    public class SQClause : BasePOSType
    {
        private SQClause()
        {
        }

        public override string Description => "within SBARQ (SBARQ consists of wh-element and SQ), labels yes/no question, and tag question";

        public override WordType WordType => WordType.Sentence;

        public override bool IsGroup => true;

        public static SQClause Instance { get; } = new SQClause();

        public override string Tag => "SQ";
    }
}
