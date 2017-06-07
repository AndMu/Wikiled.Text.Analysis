namespace Wikiled.Text.Analysis.POS.Tags
{
    public class ParticlePhrase : BasePOSType
    {
        private readonly static ParticlePhrase instance = new ParticlePhrase();

        private ParticlePhrase()
        {
        }

        public override string Description
        {
            get { return "Particle"; }
        }

        public override WordType WordType
        {
            get { return WordType.Unknown; }
        }

        public override bool IsGroup
        {
            get
            {
                return true;
            }
        }

        public static ParticlePhrase Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "PRT"; }
        }
    }
}

