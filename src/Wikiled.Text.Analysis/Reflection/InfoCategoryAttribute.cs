using System;
using Wikiled.Common.Arguments;

namespace Wikiled.Text.Analysis.Reflection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Class)]
    public class InfoCategoryAttribute : Attribute
    {
        public InfoCategoryAttribute(string name)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Name = name;
        }

        public bool Ignore { get; set; }

        public bool IsCollapsed { get; set; }

        public string Name { get; }
    }
}
