namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PrepositionSubordinateConjunction : BasePOSType
    {
        private PrepositionSubordinateConjunction()
        {
        }

        public override string Description => "Preposition/subordinate conjunction";

        public override WordType WordType => WordType.Unknown;

        public static PrepositionSubordinateConjunction Instance { get; } = new PrepositionSubordinateConjunction();

        public override string Tag => "IN";
    }
}


