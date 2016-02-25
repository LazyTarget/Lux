using System;

namespace Lux.Model
{
    public class StronglyTypedProperty : Property
    {
        public StronglyTypedProperty(string name, Type type)
            : this(name, type, null)
        {
        }

        public StronglyTypedProperty(string name, Type type, object value)
            : this(name, type, false, value)
        {
        }

        public StronglyTypedProperty(string name, Type type, bool isReadOnly, object value)
            : base(name, type, isReadOnly, value)
        {
            //if (type == null)
            //    throw new ArgumentNullException(nameof(type));
            AssertIsAssignable(value);
        }

        protected void AssertIsAssignable(object value)
        {
            //if (Type == null)
            //    throw new InvalidOperationException($"Missing required property '{this.Type}'");
            if (value != null && Type != null)
            {
                var type = value.GetType();
                var valid = this.Type.IsAssignableFrom(type);
                if (!valid)
                    throw new InvalidOperationException("Invalid property value. Doesn't match the required type");
            }
        }


        public override void SetValue(object value)
        {
            AssertIsAssignable(value);
            base.SetValue(value);
        }
    }
}
