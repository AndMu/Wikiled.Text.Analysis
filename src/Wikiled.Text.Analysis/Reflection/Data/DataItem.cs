
using System;

namespace Wikiled.Text.Analysis.Reflection.Data
{
    public class DataItem : IDataItem
    {
        public DataItem(string category, string name, string description, object value)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            Category = category;
            Value = value;
            Name = name;
            Description = description;
        }

        public string Category { get; }

        public string Description { get; }

        public string FullName => Category + " " + Name;

        public string Name { get; }

        public object Value { get; set; }
    }
}
