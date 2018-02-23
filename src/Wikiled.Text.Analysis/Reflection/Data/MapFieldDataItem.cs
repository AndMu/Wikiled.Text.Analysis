using Wikiled.Common.Arguments;

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
            Guard.NotNull(() => field, field);
            Guard.NotNull(() => parent, parent);
            Guard.NotNull(() => parent.Instance, parent.Instance);
            this.field = field;
            this.parent = parent;
            instance = parent.Instance;
            currentValue = field.GetValue<object>(instance);
        }

        public string FullName => parent.Name + " " + Name;

        public string Name => field.Name;

        public string Description => field.Description;

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
