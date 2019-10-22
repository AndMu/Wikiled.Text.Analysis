using System;

namespace Wikiled.Text.Analysis.Structure.Light
{
    public interface IDocument
    {
        string Text { get; set; }

        string Author { get; set; }

        string Id { get; set; }

        DateTime? DocumentTime { get; set; }

        string Title { get; set; }
    }
}