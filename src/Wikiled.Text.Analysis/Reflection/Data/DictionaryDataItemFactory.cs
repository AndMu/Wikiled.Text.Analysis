﻿using System;
using System.Collections.Generic;
using NLog;

namespace Wikiled.Text.Analysis.Reflection.Data
{
    public class DictionaryDataItemFactory : IDataItemFactory
    {
        private static readonly Logger log = LogManager.GetCurrentClassLogger();

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
                    log.Warn("{0} value not found", field.Name);
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
