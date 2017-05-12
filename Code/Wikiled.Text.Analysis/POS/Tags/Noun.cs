namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Noun : BasePOSType
    {
        private Noun()
        {
        }

        public static Noun Instance { get; } = new Noun();

        public override string Description => "Noun, singular or mass";

        public override string Tag => "NN";

        public override WordType WordType => WordType.Noun;
    }
}
