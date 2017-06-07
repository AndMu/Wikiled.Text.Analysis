namespace Wikiled.Text.Analysis.POS.Tags
{
    public class VerbPastTense : BasePOSType
    {
        private VerbPastTense()
        {
        }

        public override string Description => "Verb, past tense";

        public override WordType WordType => WordType.Verb;

        public static VerbPastTense Instance { get; } = new VerbPastTense();

        public override string Tag => "VBD";
    }
}


