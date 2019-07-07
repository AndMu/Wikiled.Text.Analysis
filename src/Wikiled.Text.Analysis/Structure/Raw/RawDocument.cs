using System;
using System.Text;

namespace Wikiled.Text.Analysis.Structure.Raw
{
    public class RawDocument
    {
        public RawPage[] Pages { get; set; }

        public string Build()
        {
            var builder = new StringBuilder();
            if (Pages != null)
            {
                foreach (var rawPage in Pages)
                {
                    if (rawPage.Blocks != null)
                    {
                        foreach (var block in rawPage.Blocks)
                        {
                            if (builder.Length > 0)
                            {
                                builder.Append(Environment.NewLine);
                            }

                            builder.Append(block.Text);
                        }
                    }
                }
            }

            return builder.ToString();
        }
    }
}
