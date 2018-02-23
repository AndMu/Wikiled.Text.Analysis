using System;
using Wikiled.Common.Arguments;

namespace Wikiled.Text.Analysis.Reflection
{
    [AttributeUsage(AttributeTargets.Property)]
    public class InfoArrayCategoryAttribute : Attribute
    {
        public InfoArrayCategoryAttribute(string name, string textField, string valueField)
        {
            Guard.NotNullOrEmpty(() => name, name);
            Guard.NotNullOrEmpty(() => textField, textField);
            Guard.NotNullOrEmpty(() => valueField, valueField);
            Name = name;
            TextField = textField;
            ValueField = valueField;
        }

        public string Name { get; }

        public string TextField { get; }

        public string ValueField { get; }
    }
}
