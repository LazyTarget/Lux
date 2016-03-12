using System.Collections.Generic;

namespace Lux.Model
{
    public interface IObjectModel : IModel
    {
        IEnumerable<IProperty> GetProperties();
        IProperty GetProperty(string name);
        //void DefineProperty(IProperty property);
        void SetPropertyValue(string name, object value);
        void ClearProperties();
    }
}
