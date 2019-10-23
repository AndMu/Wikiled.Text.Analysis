using System;

namespace Wikiled.Text.Analysis.Structure.Light
{
    public class LightSentence
    {
        public LightWord[] Words { get; set; } = Array.Empty<LightWord>();

        public string Text { get; set; }
    }
}
