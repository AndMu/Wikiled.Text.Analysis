namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IModelReader
    {
        bool CaseSensitive { get; set; }

        IWordModel Open();
    }
}