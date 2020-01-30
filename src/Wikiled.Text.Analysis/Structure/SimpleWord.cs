using System;

namespace Wikiled.Text.Analysis.Structure
{
    public class SimpleWord : IItem
    {
        public SimpleWord(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(text));
            }

            Text = text;
        }
        
        public string Text { get; }
    }
}
