﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace Lux.Serialization
{
    public class ObjectMirror : IObject
    {
        private bool _isLoading;
        private readonly object _instance;
        private readonly IDictionary<string, IProperty> _properties;
        private BindingFlags _bindingFlags = BindingFlags.GetProperty | BindingFlags.Instance | BindingFlags.Public;

        public ObjectMirror(object instance)
        {
            if (instance == null)
                throw new ArgumentNullException(nameof(instance));

            _instance = instance;
            var dict = new DictionaryEx<string, IProperty>();
            dict.OnPairChanged += Properties_OnPairChanged;
            _properties = dict;

            Load();
        }


        private void Properties_OnPairChanged(object sender, DictionaryPairChangedEventArgs<string, IProperty> args)
        {
            if (_isLoading)
                return;
            if (!args.Removed)
                SetPropertyValue(args.Key, args.Value);
            else
            {
                // remove property from object??
            }
        }

        protected void Load()
        {
            _isLoading = true;
            var type = _instance.GetType();
            var properties = type.GetProperties(_bindingFlags).ToList();

            foreach (var propertyInfo in properties)
            {
                var propertyName = propertyInfo.Name;
                if (string.IsNullOrEmpty(propertyName))
                    continue;

                var property = MirrorProperty.Create(_instance, propertyName);
                _properties[propertyName] = property;
            }
            _isLoading = false;
        }



        public IEnumerable<string> GetPropertyNames()
        {
            return GetProperties().Where(x => x != null).Select(x => x.Name);
        }

        public IEnumerable<IProperty> GetProperties()
        {
            return _properties.Values.Where(x => x != null);
        }

        public bool HasProperty(string name)
        {
            var res = GetPropertyNames().Contains(name);
            return res;
        }

        public IProperty GetProperty(string name)
        {
            IProperty res = null;
            var hasProp = HasProperty(name);
            if (hasProp)
            {
                res = _properties[name];
            }
            return res;
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
                throw new KeyNotFoundException($"Property '{name}' not found");
            }
        }

        public void ClearProperties()
        {
            _properties.Clear();
        }

    }
}
