namespace Wikiled.Text.Analysis.POS.Tags
{
    public class Modal : BasePOSType
    {
        private Modal()
        {

        }

        public override string Description => "Modal";

        public override WordType WordType => WordType.Unknown;

        public static Modal Instance { get; } = new Modal();

        public override string Tag => "MD";
    }
}
