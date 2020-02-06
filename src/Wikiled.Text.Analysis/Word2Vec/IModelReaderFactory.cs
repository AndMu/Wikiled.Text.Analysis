namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IModelReaderFactory
    {
        IWordModel Construct(string filePath);
    }
}