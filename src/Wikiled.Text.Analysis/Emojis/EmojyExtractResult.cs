using System;
using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Emojis
{
    public class EmojyExtractResult
    {
        private List<Emoji> emojis = new List<Emoji>();

        public EmojyExtractResult(string original)
        {
            Original = original;
        }

        public string Original { get; }

        public string Cleaned { get; set; }

        public IEnumerable<Emoji> Emojis => emojis;

        public void AddEmoji(Emoji emoji)
        {
            if (emoji == null)
            {
                throw new ArgumentNullException(nameof(emoji));
            }

            emojis.Add(emoji);
        }
    }
}
