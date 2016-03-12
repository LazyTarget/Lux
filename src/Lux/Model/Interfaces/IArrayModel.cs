using System.Collections;
using System.Collections.Generic;

namespace Lux.Model
{
    public interface IArrayModel : IModel
    {
        IEnumerable<object> Items();

        void AddItem(object item);
        void ClearItems();
    }
    
    
    public interface IArrayModel<TItem> : IArrayModel
    {
        new IEnumerable<TItem> Items();

        void AddItem(TItem item);
        //void ClearItems();
    }
}
