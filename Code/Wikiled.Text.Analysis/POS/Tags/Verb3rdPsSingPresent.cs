namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Verb3rdPsSingPresent : BasePOSType
    {
        private readonly static Verb3rdPsSingPresent instance = new Verb3rdPsSingPresent();

        private Verb3rdPsSingPresent()
        {
        }

        public override string Description
        {
            get { return "Verb, 3rd ps. sing. present"; }
        }

        public override WordType WordType
        {
            get { return WordType.Verb; }
        }

        public static Verb3rdPsSingPresent Instance
        {
            get { return instance; }
        }

        public override string Tag
        {
            get { return "VBZ"; }
        }
    }
}
