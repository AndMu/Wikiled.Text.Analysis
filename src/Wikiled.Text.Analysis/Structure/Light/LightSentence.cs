using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Structure.Light
{
    public class LightSentence
    {
        public List<LightWord> Words { get; set; } = new List<LightWord>();

        public string Text { get; set; }
    }
}
