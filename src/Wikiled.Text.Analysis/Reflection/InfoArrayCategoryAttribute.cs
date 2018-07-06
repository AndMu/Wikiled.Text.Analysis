using System;

namespace Wikiled.Text.Analysis.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InfoArrayCategoryAttribute : Attribute
    {
        public InfoArrayCategoryAttribute(string name, string textField, string valueField)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            if (string.IsNullOrEmpty(textField))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(textField));
            }

            if (string.IsNullOrEmpty(valueField))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(valueField));
            }

            Name = name;
            TextField = textField;
            ValueField = valueField;
        }

        public string Name { get; }

        public string TextField { get; }

        public string ValueField { get; }
    }
}
