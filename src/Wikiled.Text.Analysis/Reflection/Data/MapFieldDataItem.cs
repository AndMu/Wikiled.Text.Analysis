
using System;

namespace Wikiled.Text.Analysis.Reflection.Data
{
    public class MapFieldDataItem : IDataItem
    {
        private readonly IMapField field;

        private readonly object instance;

        private readonly IDataTree parent;

        private object currentValue;

        public MapFieldDataItem(IDataTree parent, IMapField field)
        {
            this.field = field ?? throw new ArgumentNullException(nameof(field));
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            instance = parent.Instance ?? throw new ArgumentNullException(nameof(parent.Instance));
            currentValue = field.GetValue<object>(instance);
        }

        public string FullName => parent.Name + " " + Name;

        public string Name => field.Name;

        public string Description => field.Description ?? field.PropertyName;

        public object Value
        {
            get => currentValue;
            set
            {
                currentValue = value;
                field.SetValue(instance, value);
            }
        }
    }
}
