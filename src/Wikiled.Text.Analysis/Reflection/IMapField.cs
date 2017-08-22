using System;

namespace Wikiled.Text.Analysis.Reflection
{
    public interface IMapField
    {
        IMapCategory Category { get; }

        string Description { get; }

        bool IsOptional { get; }

        string Name { get; }

        int Order { get; }

        Type ValueType { get; }

        T GetValue<T>(object instance);

        void SetValue(object instance, object value);
    }
}
