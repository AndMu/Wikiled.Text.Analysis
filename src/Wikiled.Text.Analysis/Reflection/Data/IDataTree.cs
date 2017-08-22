using System.Collections.Generic;

namespace Wikiled.Text.Analysis.Reflection.Data
{
    public interface IDataTree
    {
        IEnumerable<IDataItem> AllLeafs { get; }

        IList<IDataItem> Leafs { get; }

        IList<IDataTree> Branches { get; }

        object Instance { get; }

        string Name { get; }

        string FullName { get; }
    }
}
