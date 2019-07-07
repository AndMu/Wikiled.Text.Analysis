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
                for (var i = 0; i < Pages.Length; i++)
                {
                    var text = Pages[i].Build();
                    if (i < (Pages.Length - 1))
                    {
                        builder.AppendLine(text);
                    }
                    else
                    {
                        builder.Append(text);
                    }
                }
            }

            return builder.ToString();
        }
    }
}
