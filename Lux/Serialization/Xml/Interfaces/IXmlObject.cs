﻿using System.Collections.Generic;

namespace Lux.Serialization.Xml
{
    public interface IXmlObject : IXmlNode, IObject
    {
        //IEnumerable<string> GetPropertyNames();
        //IEnumerable<IProperty> GetProperties(); 
        //bool HasProperty(string name);
        //IProperty GetProperty(string name);
        //void SetPropertyValue(string name, object value);
        //void ClearProperties();
    }
}