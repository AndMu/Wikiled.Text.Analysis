using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Wikiled.Common.Reflection;
using Wikiled.Text.Analysis.Helpers;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.Reflection
{
    public class EnumerableMapCategory : ChildMapCategory
    {
        private readonly IMapCategory category;

        private readonly Dictionary<Type, bool> typeMap = new Dictionary<Type, bool>();

        public EnumerableMapCategory(IMapCategory parent, string name, PropertyInfo propertyInfo)
            : base(parent, name, propertyInfo)
        {
            if (!typeof(IEnumerable).IsAssignableFrom(propertyInfo.PropertyType))
            {
                throw new ArgumentException(nameof(propertyInfo.PropertyType));
            }

            CategoriesMapper mapper = new CategoriesMapper();
            var type = propertyInfo.PropertyType.IsGenericType
                ? propertyInfo.PropertyType.GetGenericArguments().First()
                : propertyInfo.PropertyType.GetElementType();
            category = mapper.Construct(type);
        }

        public override IEnumerable<IDataItem> GetOtherLeafs(object instance)
        {
            var collection = instance as IEnumerable;
            if (collection == null ||
                category == null)
            {
                yield break;
            }

            Dictionary<string, Tuple<DataItem, int>> table = new Dictionary<string, Tuple<DataItem, int>>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in collection)
            {
                AddLead(table, new DataTree(item, category));
            }

            foreach (var tuple in table)
            {
                var oldIdem = tuple.Value.Item1;
                if (typeMap[oldIdem.Value.GetType()])
                {
                    var value = Calculator<object>.Divide(oldIdem.Value, tuple.Value.Item2);
                    yield return new DataItem(oldIdem.Category, oldIdem.Name, oldIdem.Description, value);
                }
                else
                {
                    yield return oldIdem;
                }
            }
        }

        private void AddLead(Dictionary<string, Tuple<DataItem, int>> table, IDataTree tree)
        {
            foreach (var dataTree in tree.Branches)
            {
                AddLead(table, dataTree);
            }

            foreach (var leaf in tree.Leafs)
            {
                var type = leaf.Value.GetType();
                if (!typeMap.TryGetValue(type, out bool canUse))
                {
                    canUse = type.IsPrimitive && type.IsNumericType();
                    typeMap[type] = canUse;
                }

                if (!canUse ||
                    !table.TryGetValue(leaf.FullName, out Tuple<DataItem, int> dataItem))
                {
                    dataItem = new Tuple<DataItem, int>(
                        new DataItem(tree.FullName, leaf.Name, leaf.Description, leaf.Value),
                        1);
                }
                else
                {
                    var value = Calculator<object>.Add(dataItem.Item1.Value, leaf.Value);
                    dataItem = new Tuple<DataItem, int>(
                        new DataItem(tree.FullName, leaf.Name, leaf.Description, value),
                        dataItem.Item2 + 1);
                }

                table[leaf.FullName] = dataItem;
            }
        }
    }
}
