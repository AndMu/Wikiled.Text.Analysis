using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Wikiled.Common.Logging;

namespace Wikiled.Text.Analysis.Reflection.Data
{
    public class DictionaryDataItemFactory : IDataItemFactory
    {
        private static readonly ILogger log = ApplicationLogging.CreateLogger<DictionaryDataItemFactory>();

        private readonly Dictionary<string, double> map;

        public DictionaryDataItemFactory(Dictionary<string, double> map)
        {
            this.map = map ?? throw new ArgumentNullException(nameof(map));
        }

        public IDataItem Create(IDataTree tree, IMapField field)
        {
            if (!map.TryGetValue(field.Name, out double value))
            {
                if (!field.IsOptional)
                {
                    log.LogWarning("{0} value not found", field.Name);
                }
            }

            return new DataItem(tree.Name, field.Name, field.Description, value);
        }

        public IDataTree Create(IDataTree tree, IMapCategory mapCategory)
        {
            return new DataTree(map, mapCategory, this);
        }
    }
}
