namespace Wikiled.Text.Analysis.POS.Tags
{
    public class SBARQ : BasePOSType
    {
        private SBARQ()
        {
        }

        public override string Description => "wh-question";

        public override WordType WordType => WordType.Sentence;

        public override bool IsGroup => true;

        public static SBARQ Instance { get; } = new SBARQ();

        public override string Tag => "SBARQ";
    }
}
