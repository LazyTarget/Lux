using System.Collections;
using System.Collections.Generic;

namespace Lux.Serialization
{
    public interface IArray //: IEnumerable  //: INode
    {
        IEnumerable<object> Items();

        void AddItem(object item);
        void ClearItems();
    }

    
    public interface IArray<TItem> : IArray //, IEnumerable<TItem> //: INode
    {
        new IEnumerable<TItem> Items();

        void AddItem(TItem item);
        //void ClearItems();
    }
}
