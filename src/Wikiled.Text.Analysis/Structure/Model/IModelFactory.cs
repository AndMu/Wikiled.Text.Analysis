namespace Wikiled.Text.Analysis.Structure.Model
{
    public interface IModelFactory<T>
        where T : class, IModel
    {
        T Load (string path);

        void Save(string path, T model);
    }
}
