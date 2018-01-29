using System.IO;

namespace Wikiled.Text.Analysis.Dictionary.Streams
{
    public interface IStreamSource
    {
        Stream ConstructReader(string name);
    }
}