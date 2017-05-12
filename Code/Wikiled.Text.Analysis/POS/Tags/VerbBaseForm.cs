namespace Wikiled.Text.Analysis.POS.Tags
{
    public class VerbBaseForm : BasePOSType
    {
        private VerbBaseForm()
        {
        }

        public override string Description => "Verb, base form";

        public override WordType WordType => WordType.Verb;

        public static VerbBaseForm Instance { get; } = new VerbBaseForm();

        public override string Tag => "VB";
    }
}


