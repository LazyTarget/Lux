using System;
using System.Collections.Generic;
using System.Linq;
using Lux.Model;
using Newtonsoft.Json.Linq;

namespace Lux.Serialization.Json.JsonNet
{
    public class JsonObjectModel : IObjectModel
    {
        private IDictionary<string, IProperty> _properties; 

        public JsonObjectModel(JObject jObject)
            : this(jObject, new JObjectPropertyParser())
        {
            JObject = jObject;
        }

        public JsonObjectModel(JObject jObject, IJObjectPropertyParser propertyParser)
        {
            _properties = new Dictionary<string, IProperty>();
            JObject = jObject;
            PropertyParser = propertyParser;
        }

        public JObject JObject { get; private set; }

        public IJObjectPropertyParser PropertyParser { get; private set; } 


        public IEnumerable<IProperty> GetProperties()
        {
            // Use cached properties
            var result = new Dictionary<string, IProperty>();
            var parsed = PropertyParser.Parse(JObject).ToDictionary(x => x.Name, x => x);

            for (var i = 0; i < _properties.Count; i++)
            {
                var pair = _properties.ElementAt(i);
                if (!parsed.ContainsKey(pair.Key))
                {
                    // Property removed from element, remove from cache...
                    _properties.Remove(pair);
                    continue;
                }
                // Append to result set
                result[pair.Key] = pair.Value;
            }

            foreach (var property in parsed.Values)
            {
                if (result.ContainsKey(property.Name))
                {
                    // Property exists in cache, ignore...
                    continue;
                }
                else
                {
                    // New property, add to cache and result set...
                    result[property.Name] =
                        _properties[property.Name] = property;
                }
            }
            return result.Values;
        }

        public IProperty GetProperty(string name)
        {
            var property = GetProperties().FirstOrDefault(x => x.Name == name);
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
                property = PropertyParser.DefineProperty(JObject, name, type, value, false);
                _properties[property.Name] = property;
            }
        }

        public void ClearProperties()
        {
            PropertyParser.ClearProperties(JObject);
            _properties.Clear();
        }

    }
}
