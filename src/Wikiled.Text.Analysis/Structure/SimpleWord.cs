using Wikiled.Common.Arguments;

namespace Wikiled.Text.Analysis.Structure
{
    public class SimpleWord : IItem
    {
        public SimpleWord(string text)
        {
            Guard.NotNullOrEmpty(() => text, text);
            Text = text;
        }
        
        public string Text { get; }
    }
}
