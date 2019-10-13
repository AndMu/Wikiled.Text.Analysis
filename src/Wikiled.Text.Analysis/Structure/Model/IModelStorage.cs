namespace Wikiled.Text.Analysis.Structure.Model
{
    public interface IModelStorage<out T>
        where T : class, IModel
    {
        T Current { get; }

        void Reset();

        void Add(DataType type, params IProcessingTextBlock[] blocks);

        T Load(string path);

        void Save(string path);
    }
}
