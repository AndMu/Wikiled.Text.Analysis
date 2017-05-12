namespace Wikiled.Text.Analysis.POS.Tags
{
    public class VerbPastParticiple : BasePOSType
    {
        private readonly static VerbPastParticiple instance = new VerbPastParticiple();

        private VerbPastParticiple()
        {
        }

        public override string Description
        {
            get { return "Verb, past participle"; }
        }

        public override WordType WordType
        {
            get { return WordType.Verb; }
        }

        public static VerbPastParticiple Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "VBN"; }
        }
    }
}


