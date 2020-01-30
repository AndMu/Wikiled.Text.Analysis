namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IModelReaderFactory
    {
        IWordModel Contruct(string filePath);
    }
}