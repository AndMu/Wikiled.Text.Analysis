namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ClauseIntroducedByASubordinatingConjunction : BasePOSType
    {
        private ClauseIntroducedByASubordinatingConjunction()
        {
        }

        public override string Description => "Clause introduced by a subordinating conjunction";

        public override WordType WordType => WordType.Unknown;

        public override bool IsGroup => true;

        public static ClauseIntroducedByASubordinatingConjunction Instance { get; } = new ClauseIntroducedByASubordinatingConjunction();

        public override string Tag => "SBAR";
    }
}

