namespace Lux.Model
{
    public class StronglyTypedProperty<TValue> : StronglyTypedProperty
    {
        public StronglyTypedProperty(string name)
            : base(name, typeof(TValue))
        {
        }

        public StronglyTypedProperty(string name, object value)
            : base(name, typeof(TValue), value)
        {
        }

        public StronglyTypedProperty(string name, bool isReadOnly, object value)
            : base(name, typeof(TValue), isReadOnly, value)
        {
        }

        public new TValue Value { get { return (TValue)base.Value; } }
    }
}