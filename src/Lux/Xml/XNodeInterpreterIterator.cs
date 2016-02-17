using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public class XNodeInterpreterIterator<TNode, TParent> : IXNodeInterpreterIterator<TNode, TParent>
        where TNode : XNode
    {
        private readonly IXNodeInterpreter<TNode, TParent>[] _interpreters;
        private readonly TParent _parent;

        public XNodeInterpreterIterator(IXNodeInterpreter<TNode, TParent>[] interpreters, TParent parent)
        {
            _interpreters = interpreters;
            _parent = parent;
        }

        public IXNodeInterpreter<TNode, IXNodeInterpreterIterator<TNode, TParent>> GetAtIndex(int index)
        {
            var enumerable = Enumerate();
            var result = enumerable.ElementAt(index);
            return result;
        }
        
        public IEnumerable<IXNodeInterpreter<TNode, IXNodeInterpreterIterator<TNode, TParent>>> Enumerate()
        {
            foreach (var interpreter in _interpreters)
            {
                var node = interpreter.GetNode();
                var navigator = XNodeInterpreter<TNode, IXNodeInterpreterIterator<TNode, TParent>>.Create(node, this);
                yield return navigator;
            }
        }
        IEnumerable<IXNodeInterpreter<TNode>> IXNodeInterpreterIterator<TNode>.Enumerate()
        {
            return Enumerate();
        }

        IEnumerable<IXNodeInterpreter> IXNodeInterpreterIterator.Enumerate()
        {
            return Enumerate();
        }


        public TParent Return()
        {
            return _parent;
        }
    }
}