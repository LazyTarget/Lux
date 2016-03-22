using System;
using System.Collections.Generic;
using System.Linq;
using Lux.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Lux.Serialization.Json.JsonNet
{
    public class JObjectPropertyParser : IJObjectPropertyParser
    {
        public JsonSerializer JsonSerializer { get; set; }


        public IEnumerable<IProperty> Parse(JObject obj)
        {
            var properties = new List<IProperty>();
            var props = obj.Properties().Where(x => x != null).ToList();
            if (props.Any())
            {
                foreach (var prop in props)
                {
                    IProperty property = new JsonProperty(prop);
                    properties.Add(property);
                }
            }
            return properties;
        }

        public IProperty DefineProperty(JObject obj, string propertyName, Type type, object value, bool isReadOnly)
        {
            JProperty prop = obj.Property(propertyName);
            if (prop == null)
            {
                prop = new JProperty(propertyName);
                obj.Add(prop);
            }

            var property = new JsonProperty(prop, JsonSerializer);
            //property.Name = propertyName;
            property.ReadOnly = isReadOnly;
            property.Type = type;
            property.SetValue(value);
            return property;
        }

        public void ClearProperties(JObject obj)
        {
            foreach (var property in obj.Properties())
            {
                property.Remove();
            }
        }




        public class JsonProperty : IProperty
        {
            private readonly Assignable<string> _name = new Assignable<string>(); 
            private readonly Assignable<object> _value = new Assignable<object>();
            private readonly Assignable<Type> _type = new Assignable<Type>(); 
            private readonly JProperty _property;
            private readonly JsonSerializer _jsonSerializer;

            public JsonProperty(JProperty property)
            {
                if (property == null)
                    throw new ArgumentNullException(nameof(property));
                _property = property;
            }

            public JsonProperty(JProperty property, JsonSerializer jsonSerializer)
                : this(property)
            {
                _jsonSerializer = jsonSerializer;
            }


            public string Name
            {
                get
                {
                    string value;
                    if (_name.Assigned)
                    {
                        value = _name.Value;
                    }
                    else
                    {
                        value = _property?.Name;
                    }
                    return value;
                }
            }

            public Type Type
            {
                get { return _type.Value; }
                set { _type.Value = value; }
            }

            public bool ReadOnly
            {
                get;
                set;
            }

            public object Value
            {
                get
                {
                    object value;
                    if (_value.Assigned)
                    {
                        value = _value.Value;
                    }
                    else
                    {
                        if (_property?.Value == null)
                        {
                            value = null;
                        }
                        else if (_property.Type == JTokenType.Null)
                        {
                            value = null;
                        }
                        else if (_property.Value is JValue)
                        {
                            var val = (JValue)_property.Value;
                            value = val.Value;
                        }
                        else
                        {
                            if (Type != null)
                                value = _property.Value.ToObject(Type);
                            else
                                value = _property.Value;
                        }

                        AssertIsAssignable(value);
                        _value.Value = value;
                    }
                    return value;
                }
                private set
                {
                    AssertIsAssignable(value);
                    _value.Value = value;

                    if (value != null)
                    {
                        var token = _jsonSerializer != null
                                        ? JToken.FromObject(value, _jsonSerializer)
                                        : JToken.FromObject(value);
                        _property.Value = token;
                    }
                    else
                    {
                        _property.Value = null;
                    }
                }
            }

            public void SetValue(object value)
            {
                Value = value;
            }


            protected void AssertIsAssignable(object value)
            {
                if (value != null && Type != null)
                {
                    var type = value.GetType();
                    var valid = this.Type.IsAssignableFrom(type);
                    if (!valid)
                        throw new InvalidOperationException("Invalid property value. Doesn't match the required type");
                }
            }

        }
    }
}