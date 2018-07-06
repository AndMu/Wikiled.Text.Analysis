using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.Reflection
{
    public class CollectionMapCategory : ChildMapCategory
    {
        public CollectionMapCategory(IMapCategory parent, InfoArrayCategoryAttribute attribute, PropertyInfo propertyInfo)
            : base(parent, attribute.Name, propertyInfo)
        {
            Attribute = attribute;
        }

        public override IMapField[] Fields { get; } = { };

        public InfoArrayCategoryAttribute Attribute { get; }

        public override IEnumerable<IDataItem> GetOtherLeafs(object instance)
        {
            var collection = (IEnumerable)instance;
            PropertyInfo textProperty = null;
            PropertyInfo valueProperty = null;
            foreach (var item in collection)
            {
                if (textProperty == null)
                {
                    Type itemType = item.GetType();
                    textProperty = itemType.GetProperty(Attribute.TextField);
                    valueProperty = itemType.GetProperty(Attribute.ValueField);
                    if (textProperty == null)
                    {
                        throw new ArgumentNullException(nameof(textProperty));
                    }

                    if (valueProperty == null)
                    {
                        throw new ArgumentNullException(nameof(valueProperty));
                    }
                }

                string name = (string)textProperty.GetValue(item, null);
                object value = valueProperty.GetValue(item, null);
                DataItem dataItemitem = new DataItem(Name, name, name, value);
                yield return dataItemitem;
            }
        }

        protected override IEnumerable<IMapField> ConstructAllChildFields()
        {
            yield break;
        }
    }
}
