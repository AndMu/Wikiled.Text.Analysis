namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Particle : BasePOSType
    {
        private Particle()
        {

        }

        public override string Description => "Particle";

        public override WordType WordType => WordType.Unknown;

        public static Particle Instance { get; } = new Particle();

        public override string Tag => "RP";
    }
}
