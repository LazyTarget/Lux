using System.Collections;

namespace Lux.Serialization
{
    public interface IArray : INode, IEnumerable
    {
        void AddItem(object item);
        void Clear();
    }
}
