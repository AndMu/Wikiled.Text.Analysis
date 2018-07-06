using System;

namespace Wikiled.Text.Analysis.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class InfoCategoryAttribute : Attribute
    {
        public InfoCategoryAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            Name = name;
        }

        public bool Ignore { get; set; }

        public bool IsCollapsed { get; set; }

        public string Name { get; }
    }
}
