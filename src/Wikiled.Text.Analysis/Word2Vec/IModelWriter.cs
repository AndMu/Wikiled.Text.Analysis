using System;

namespace Wikiled.Text.Analysis.Word2Vec
{
    public interface IModelWriter : IDisposable
    {
        void Write(Model m);
    }
}