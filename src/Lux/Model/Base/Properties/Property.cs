using System;

namespace Lux.Model
{
    public class Property : IProperty
    {
        public Property(string name)
            : this(name, null)
        {
        }

        public Property(string name, Type type)
            : this(name, type, null)
        {
        }

        public Property(string name, Type type, object value)
            : this(name, type, false, value)
        {
        }

        public Property(string name, Type type, bool isReadOnly, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            Name = name;
            Type = type;
            ReadOnly = isReadOnly;
            Value = value;
        }


        public virtual string Name { get; }
        public virtual Type Type { get; }
        public virtual bool ReadOnly { get; }
        public virtual object Value { get; private set; }


        public virtual void SetValue(object value)
        {
            if (ReadOnly)
                throw new InvalidOperationException("Property is readonly!");
            Value = value;
        }
    }
}
