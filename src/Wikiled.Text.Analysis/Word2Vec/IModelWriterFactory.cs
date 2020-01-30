namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IModelWriterFactory
    {
        IModelWriter Contruct(string filePath);
    }
}