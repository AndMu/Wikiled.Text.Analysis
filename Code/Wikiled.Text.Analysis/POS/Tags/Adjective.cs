namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Adjective : BasePOSType
    {
        private Adjective()
        {
        }

        public override string Description => "Adjective";

        public override WordType WordType => WordType.Adjective;

        public static Adjective Instance { get; } = new Adjective();

        public override string Tag => "JJ";
    }
}


