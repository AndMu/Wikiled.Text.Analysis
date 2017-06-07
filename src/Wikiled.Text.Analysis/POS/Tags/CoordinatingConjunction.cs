namespace Wikiled.Text.Analysis.POS.Tags
{
    public class CoordinatingConjunction : BasePOSType
    {
        private CoordinatingConjunction()
        {
        }

        public override string Description => "Coordinating conjunction";

        public override WordType WordType => WordType.Conjunction;

        public static CoordinatingConjunction Instance { get; } = new CoordinatingConjunction();

        public override string Tag => "CC";
    }
}
