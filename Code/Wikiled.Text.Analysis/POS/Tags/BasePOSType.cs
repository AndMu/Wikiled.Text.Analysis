namespace Wikiled.Text.Analysis.POS.Tags
{
    public abstract class BasePOSType
    {
        public abstract string Description { get; }

        public abstract string Tag { get; }

        public abstract WordType WordType { get; }

        public virtual bool IsGroup => false;
    }
}
