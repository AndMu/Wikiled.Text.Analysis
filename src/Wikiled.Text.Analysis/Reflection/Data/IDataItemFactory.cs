namespace Wikiled.Text.Analysis.Reflection.Data
{
    public interface IDataItemFactory
    {
        IDataItem Create(IDataTree tree, IMapField field);

        IDataTree Create(IDataTree tree, IMapCategory mapCategory);
    }
}