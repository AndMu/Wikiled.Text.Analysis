using System.Reflection;

namespace Wikiled.Text.Analysis.Reflection
{
    public class ChildMapCategory : MapCategory
    {
        private readonly string name;
        private readonly PropertyInfo propertyInfo;

        public ChildMapCategory(IMapCategory parent, string name, PropertyInfo propertyInfo)
            : base(parent.IsPropertyName, name, propertyInfo.PropertyType)
        {
            Parent = parent;
            this.name = name;
            this.propertyInfo = propertyInfo;
        }

        public override string FullName => Parent.FullName + " " + base.FullName;

        public override IMapCategory Parent { get; }

        public override object ResolveInstance(object parent)
        {
            return propertyInfo.GetValue(parent, null);
        }
    }
}
