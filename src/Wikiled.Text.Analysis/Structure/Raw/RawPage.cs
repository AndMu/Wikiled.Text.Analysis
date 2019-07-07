using System;
using System.Text;

namespace Wikiled.Text.Analysis.Structure.Raw
{
    public class RawPage
    {
        public TextBlockItem[] Blocks { get; set; }

        public string Build()
        {
            var builder = new StringBuilder();
            if (Blocks != null)
            {
                foreach (var block in Blocks)
                {
                    if (builder.Length > 0)
                    {
                        builder.Append(Environment.NewLine);
                    }

                    builder.Append(block.Text);
                }
            }

            return builder.ToString();
        }
    }
}
