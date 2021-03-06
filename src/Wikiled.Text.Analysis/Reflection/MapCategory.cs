﻿using System;
using System.Collections.Generic;
using System.Linq;
using Wikiled.Common.Extensions;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.Reflection
{
    public class MapCategory : IMapCategory
    {
        private readonly List<IMapCategory> categories = new List<IMapCategory>();

        private readonly List<IMapField> fields = new List<IMapField>();

        private Lazy<IMapField[]> allChildFields;

        private Dictionary<string, List<IMapField>> fieldMap;

        private Lazy<Dictionary<string, List<IMapField>>> map;

        private Lazy<IMapField[]> sortedFields;

        public MapCategory(bool isPropertyName, string name, Type type)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Value cannot be null or empty.", nameof(name));
            }

            IsPropertyName = isPropertyName;
            Name = name;
            OwnerType = type ?? throw new ArgumentNullException(nameof(type));
            Reset();
        }

        public virtual IEnumerable<IMapCategory> Categories
        {
            get
            {
                foreach (var mapCategory in categories)
                {
                    if (!mapCategory.IsCollapsed)
                    {
                        yield return mapCategory;
                    }
                    else
                    {
                        foreach (var childCategory in mapCategory.Categories)
                        {
                            yield return childCategory;
                        }
                    }
                }
            }
        }

        public virtual IMapField[] Fields => sortedFields.Value;

        public virtual string FullName => Name;

        public virtual IMapCategory Parent => null;

        public string[] ActualProperties => FieldMap.Keys.ToArray();

        public IMapField[] AllChildFields => allChildFields.Value;

        public bool IsCollapsed { get; set; }

        public bool IsPropertyName { get; }

        public string Name { get; }

        public Type OwnerType { get; }

        private Dictionary<string, List<IMapField>> FieldMap => map.Value;

        public IEnumerable<IMapField> this[string name] => FieldMap[name];

        public virtual IEnumerable<IDataItem> GetOtherLeafs(object instance)
        {
            yield break;
        }

        public virtual object ResolveInstance(object parent)
        {
            return parent;
        }

        public void AddCategory(IMapCategory mapCategory)
        {
            categories.Add(mapCategory);
            Reset();
        }

        public void AddField(IMapField field)
        {
            fields.Add(field);
            Reset();
        }

        public bool ContainsField(string name)
        {
            return FieldMap.ContainsKey(name);
        }

        protected virtual IEnumerable<IMapField> ConstructAllChildFields()
        {
            foreach (var field in Fields)
            {
                yield return field;
            }

            foreach (var mapCategory in categories)
            {
                foreach (var field in mapCategory.AllChildFields)
                {
                    yield return field;
                }
            }
        }

        private Dictionary<string, List<IMapField>> ConstructMap()
        {
            fieldMap = new Dictionary<string, List<IMapField>>();
            foreach (var field in AllChildFields)
            {
                fieldMap.GetSafeCreate(field.Name)
                        .Add(field);
            }

            return fieldMap;
        }

        private void Reset()
        {
            if (map == null ||
                map.IsValueCreated)
            {
                map = new Lazy<Dictionary<string, List<IMapField>>>(ConstructMap);
            }

            if (allChildFields == null ||
                allChildFields.IsValueCreated)
            {
                allChildFields = new Lazy<IMapField[]>(
                    () => ConstructAllChildFields()
                        .ToArray());
            }

            if (sortedFields == null ||
                sortedFields.IsValueCreated)
            {
                sortedFields = new Lazy<IMapField[]>(
                    () => fields.OrderBy(item => item.Order)
                                .ToArray());
            }
        }
    }
}
