using System.Collections;
using System.Collections.Generic;

namespace Lux.Serialization
{
    public interface IArray : INode
    {
        IEnumerable<INode> Items();

        void AddItem(object item);
        void ClearItems();
    }
}
