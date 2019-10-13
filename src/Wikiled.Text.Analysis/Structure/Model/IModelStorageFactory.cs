namespace Wikiled.Text.Analysis.Structure.Model
{
    public interface IModelStorageFactory<T>
        where T : class, IModel
    {
        IModelStorage<T> Construct(string name);

        void Save(string name, IModelStorage<T> storage);
    }
}
