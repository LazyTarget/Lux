using System.Collections.Generic;
using Lux.Model;

namespace Lux.Serialization
{
    //public interface IObject //: INode
    //{
    //    IEnumerable<string> GetPropertyNames();
    //    IEnumerable<IProperty> GetProperties();
    //    bool HasProperty(string name);
    //    IProperty GetProperty(string name);
    //    //void DefineProperty(IProperty property);
    //    void SetPropertyValue(string name, object value);
    //    void ClearProperties();
    //}

    public interface IObject : IObjectModel
    {
    }
}
