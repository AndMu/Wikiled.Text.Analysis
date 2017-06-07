namespace Wikiled.Text.Analysis.POS.Tags
{
    public class CardinalNumber : BasePOSType
    {
        private CardinalNumber()
        {
        }

        public override string Description => "Cardinal number";

        public override WordType WordType => WordType.Unknown;

        public static CardinalNumber Instance { get; } = new CardinalNumber();

        public override string Tag => "CD";
    }
}
