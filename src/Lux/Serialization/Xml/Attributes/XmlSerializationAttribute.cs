using System;

namespace Lux.Serialization.Xml
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class XmlSerializationAttribute : Attribute
    {
        public Type Serializer { get; set; }
        public bool UseCustomSerializer { get; set; }
    }
}
