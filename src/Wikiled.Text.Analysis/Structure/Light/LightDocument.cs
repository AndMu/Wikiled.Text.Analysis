using System;
using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure.Light
{
    public class LightDocument : IDocument
    {
        public LightSentence[] Sentences { get; set; } = Array.Empty<LightSentence>();

        public string Text { get; set; }

        public string Author { get; set; }

        public string Id { get; set; }

        public DateTime? DocumentTime { get; set; }

        public string Title { get; set; }
    }
}
