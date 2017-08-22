using System;
using Wikiled.Core.Utility.Arguments;

namespace Wikiled.Text.Analysis.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InfoFieldAttribute : Attribute
    {
        public InfoFieldAttribute(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Name = name;
            Order = 999;
        }

        public string Description { get; set; }

        public bool IsOptional { get; set; }

        public string Name { get; }

        public int Order { get; }
    }
}
