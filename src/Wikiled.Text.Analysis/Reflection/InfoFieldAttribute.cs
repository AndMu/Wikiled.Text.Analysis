using System;

namespace Wikiled.Text.Analysis.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InfoFieldAttribute : Attribute
    {
        public InfoFieldAttribute(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            Name = name;
            Order = 999;
        }

        public string Description { get; set; }

        public bool IsOptional { get; set; }

        public string Name { get; }

        public int Order { get; }
    }
}
