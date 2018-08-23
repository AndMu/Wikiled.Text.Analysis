using System.Text;

namespace Wikiled.Text.Analysis.Structure.Raw
{
    public class TrackingBlockItem
    {
        private readonly StringBuilder builder = new StringBuilder();

        public TextBlockItem Construct()
        {
            if (builder.Length == 0)
            {
                return null;
            }

            return new TextBlockItem { Text = builder.ToString() };
        }

        public void AddText(string text)
        {
            builder.Append(text);
        }
    }
}
