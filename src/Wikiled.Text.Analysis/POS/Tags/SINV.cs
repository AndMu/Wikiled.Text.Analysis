namespace Wikiled.Text.Analysis.POS.Tags
{
    public class SINV : BasePOSType
    {
        private SINV()
        {
        }

        public override string Description => "subject-auxiliary inversion (not used with questions)";

        public override WordType WordType => WordType.Sentence;

        public override bool IsGroup => true;

        public static SINV Instance { get; } = new SINV();

        public override string Tag => "SINV";
    }
}
