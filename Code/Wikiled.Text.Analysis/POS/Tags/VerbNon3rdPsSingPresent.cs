namespace Wikiled.Text.Analysis.POS.Tags
{
    public class VerbNon3rdPsSingPresent : BasePOSType
    {
        private readonly static VerbNon3rdPsSingPresent instance = new VerbNon3rdPsSingPresent();

        private VerbNon3rdPsSingPresent()
        {

        }

        public override string Description
        {
            get { return "Verb, non-3rd ps. sing. present"; }
        }

        public override WordType WordType
        {
            get { return WordType.Verb; }
        }

        public static VerbNon3rdPsSingPresent Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "VBP"; }
        }
    }
}

