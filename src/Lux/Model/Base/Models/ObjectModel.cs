using System;
using System.Collections.Generic;
using System.Linq;

namespace Lux.Model
{
    public class ObjectModel : IObjectModel
    {
        private readonly IDictionary<string, IProperty> _properties;

        public ObjectModel()
        {
            _properties = new Dictionary<string, IProperty>();
        }

        public IEnumerable<IProperty> GetProperties()
        {
            IEnumerable<IProperty> properties = _properties.Values;
            return properties;
        }

        public IProperty GetProperty(string name)
        {
            var property = GetProperties().FirstOrDefault(x => x.Name == name);
            return property;
        }

        public virtual IProperty DefineProperty(string propertyName, Type type, object value, bool isReadOnly)
        {
            var property = new Property(propertyName, type, isReadOnly, value);
            _properties[propertyName] = property;
            return property;
        }

        public void SetPropertyValue(string name, object value)
        {
            var property = GetProperty(name);
            if (property != null)
            {
                property.SetValue(value);
            }
            else
            {
                Type type = null;
                property = DefineProperty(name, type, value, false);
                _properties[property.Name] = property;
            }
        }

        public void ClearProperties()
        {
            _properties.Clear();
        }
    }
}
