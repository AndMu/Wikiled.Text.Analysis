using System;
using System.Collections.Generic;
using Wikiled.Text.Analysis.Reflection.Data;

namespace Wikiled.Text.Analysis.Reflection
{
    public interface IMapCategory
    {
        string[] ActualProperties { get; }

        IMapField[] AllChildFields { get; }

        IEnumerable<IMapCategory> Categories { get; }

        IMapField[] Fields { get; }

        bool IsCollapsed { get; set; }

        bool IsPropertyName { get; }

        string FullName { get; }

        string Name { get; }

        Type OwnerType { get; }

        IMapCategory Parent { get; }

        IEnumerable<IMapField> this[string name] { get; }

        void AddCategory(IMapCategory category);

        void AddField(IMapField field);

        bool ContainsField(string name);

        object ResolveInstance(object parent);

        IEnumerable<IDataItem> GetOtherLeafs(object instance);
    }
}