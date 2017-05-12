namespace Wikiled.Text.Analysis.POS.Tags
{
    public class PronounPersonal : BasePOSType
    {
        private readonly static PronounPersonal instance = new PronounPersonal();

        private PronounPersonal()
        {
        }

        public override string Description
        {
            get { return "Personal pronoun"; }
        }

        public override WordType WordType
        {
            get { return WordType.Pronoun; }
        }

        public static PronounPersonal Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "PRP"; }
        }
    }
}

