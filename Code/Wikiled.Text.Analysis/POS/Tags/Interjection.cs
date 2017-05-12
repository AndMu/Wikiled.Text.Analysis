namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Interjection : BasePOSType
    {
        private Interjection()
        {
        }

        public override string Description => "Interjection";

        public override WordType WordType => WordType.Unknown;

        public static Interjection Instance { get; } = new Interjection();

        public override string Tag => "UH";
    }
}

