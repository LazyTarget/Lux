﻿using System;

namespace Lux.Serialization
{
    public class Property : IProperty
    {
        public Property(string name, Type type)
            : this(name, type, null)
        {
        }

        public Property(string name, Type type, object value)
        {
            if (string.IsNullOrEmpty(name))
                throw new ArgumentNullException(nameof(name));
            Name = name;
            Type = type;
            Value = value;
        }

        public string Name { get; }
        public Type Type { get; }
        public virtual object Value { get; private set; }

        public virtual void SetValue(object value)
        {
            Value = value;
        }
    }
}