namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ExistentialThere : BasePOSType
    {
        private ExistentialThere()
        {
        }

        public override string Description => "Existential there";

        public override WordType WordType => WordType.Unknown;

        public static ExistentialThere Instance { get; } = new ExistentialThere();

        public override string Tag => "EX";
    }
}
