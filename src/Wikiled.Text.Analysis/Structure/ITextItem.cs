namespace Wikiled.Text.Analysis.Structure
{
    public interface ITextItem : IItem
    {
        string Stemmed { get; }
    }
}
