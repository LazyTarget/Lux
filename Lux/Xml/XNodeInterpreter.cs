using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Xml
{
    public class XNodeInterpreter
    {
        public static IXNodeInterpreter<XNode> Create(XNode node)
        {
            var navigator = XNodeInterpreter<XNode, XNode>.Create(node);
            return navigator;
        }

        public static IXNodeInterpreter<TNode> Create<TNode>(TNode node)
            where TNode : XNode
        {
            var navigator = Create((XNode)node).To<TNode>();
            return navigator;
        }
    }


    public class XNodeInterpreter<TNode, TParent> : IXNodeInterpreter<TNode, TParent> //IFluentReturn<TParent>
        where TNode : XNode
    {
        protected TParent _parent;
        protected readonly TNode _node;
        protected XNodeInterpreter<XContainer, TParent> ParentInterpreter;

        /* Constructors */

        protected XNodeInterpreter(XNodeInterpreter<TNode, TParent> interpreter)
            : this(interpreter._node)
        {
            if (interpreter == null)
                throw new ArgumentNullException(nameof(interpreter));
            SetState(interpreter);
        }

        protected XNodeInterpreter(TNode node, TParent parent)
            : this(node)
        {
            _parent = parent;
        }

        protected XNodeInterpreter(TNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            _node = node;
        }


        private void SetState(XNodeInterpreter<TNode, TParent> state)
        {
            //_node = state._node;
            ParentInterpreter = state.ParentInterpreter;
            _parent = state._parent;
        }


        public static IXNodeInterpreter<XNode> Create(XNode node)
        {
            var navigator = new XNodeInterpreter<XNode, TParent>(node);
            return navigator;
        }

        internal static IXNodeInterpreter<TNode> Create(TNode node, XNodeInterpreter<XContainer, TParent> parentInterpreter = null)
        {
            var navigator = new XNodeInterpreter<TNode, TParent>(node);
            navigator.ParentInterpreter = parentInterpreter;
            navigator._parent = parentInterpreter._parent;
            return navigator;
        }



        /* Traversing */

        public TNode GetNode()
        {
            return _node;
        }

        protected T GetNode<T>()
            where T : TNode
        {
            return (T)_node;
        }

        XNode IXNodeInterpreter.GetNode()
        {
            return GetNode();
        }

        public IXNodeInterpreter<TNodeType, IXNodeInterpreter<TNode, TParent>> To<TNodeType>()
            where TNodeType : XNode
        {
            var node = (TNodeType)(object)_node;
            var navigator = new XNodeInterpreter<TNodeType, IXNodeInterpreter<TNode, TParent>>(node, this);

            //interpreter._parent = ParentInterpreter;      // important to 'restore' all state from previous object (node, parent, etc.)
            return navigator;
        }

        IXNodeInterpreter<TNodeType, IXNodeInterpreter<TNode>> IXNodeInterpreter<TNode>.To<TNodeType>()
        {
            var navigator = To<TNodeType>();
            return navigator;
        }

        IXNodeInterpreter<TNodeType, IXNodeInterpreter<XNode>> IXNodeInterpreter.To<TNodeType>()
        {
            var navigator = To<TNodeType>();
            return navigator;
        }


        public IXNodeInterpreter<XNode, IXNodeInterpreter<TNode, TParent>> GetChild(int index)
        {
            var container = (XContainer)(object)_node;
            var child = container.Nodes().ElementAt(index);

            var navigator = new XNodeInterpreter<XNode, IXNodeInterpreter<TNode, TParent>>(child, this);
            //interpreter.ParentInterpreter = ParentInterpreter;
            //interpreter._parent = this;
            //interpreter._parent = this.To<XContainer>();
            return navigator;
        }

        IXNodeInterpreter<XNode, IXNodeInterpreter<TNode>> IXNodeInterpreter<TNode>.GetChild(int index)
        {
            var navigator = GetChild(index);
            return navigator;
        }

        IXNodeInterpreter<XNode, IXNodeInterpreter<XNode>> IXNodeInterpreter.GetChild(int index)
        {
            var navigator = GetChild(index);
            return navigator;
        }

        public TParent Return()
        {
            return _parent;
        }
    }
}
