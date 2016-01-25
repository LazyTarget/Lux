using System;

namespace Lux.Serialization
{
    public class StronglyTypedProperty : Property
    {
        public StronglyTypedProperty(string name, Type type)
            : this(name, type, null)
        {
        }

        public StronglyTypedProperty(string name, Type type, object value)
            : base(name, type, value)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));
        }

        public override void SetValue(object value)
        {
            bool valid;
            if (value != null)
            {
                if (Type == null)
                    throw new InvalidOperationException($"Missing required property '{this.Type}'");

                var type = value.GetType();
                valid = this.Type.IsAssignableFrom(type);
            }
            else
                valid = true;

            if (valid)
                base.SetValue(value);
            else
                throw new InvalidOperationException("Invalid property value. Doesn't match the required type");
        }
    }
}
