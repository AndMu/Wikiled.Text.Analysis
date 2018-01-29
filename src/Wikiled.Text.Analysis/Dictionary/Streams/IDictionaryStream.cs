using System.IO;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public interface IDictionaryStream
    {
        string Name { get; }

        TextReader ConstructReadStream();
    }
}