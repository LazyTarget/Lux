using System;
using System.Linq;
using System.Xml.Linq;

namespace Lux.Xml
{
    public class XNodeInterpreter
    {
        public static IXNodeInterpreter<XNode> Create(XNode node)
        {
            var interpreter = XNodeInterpreter<XNode>.Create(node);
            return interpreter;
        }

        public static IXNodeInterpreter<TNode> Create<TNode>(TNode node)
            where TNode : XNode
        {
            var interpreter = XNodeInterpreter<TNode>.Create(node);
            return interpreter;
        }
    }


    public class XNodeInterpreter<TNode> : XNodeInterpreter<TNode, double>
        where TNode : XNode
    {
        protected XNodeInterpreter(XNodeInterpreter<TNode, double> interpreter)
            : base(interpreter)
        {
        }

        protected XNodeInterpreter(TNode node, double parent)
            : base(node, parent)
        {
        }

        protected XNodeInterpreter(TNode node)
            : base(node)
        {
        }

        
        public static IXNodeInterpreter<TNode> Create(TNode node)
        {
            var navigator = new XNodeInterpreter<TNode>(node);
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


        //public static IXNodeInterpreter<XNode> Create(XNode node)
        //{
        //    var navigator = new XNodeInterpreter<XNode, TParent>(node);
        //    return navigator;
        //}

        public static IXNodeInterpreter<TNode, TParent> Create(XNode node, TParent parent)
        {
            var n = (TNode) node;
            var interpreter = Create(n, parent);
            return interpreter;
        }

        public static IXNodeInterpreter<TNode, TParent> Create(TNode node, TParent parent)
        {
            var interpreter = new XNodeInterpreter<TNode, TParent>(node, parent);
            return interpreter;
        }

        internal static IXNodeInterpreter<TNode> Create(TNode node, XNodeInterpreter<XContainer, TParent> parentInterpreter = null)
        {
            var navigator = new XNodeInterpreter<TNode, TParent>(node);
            navigator.ParentInterpreter = parentInterpreter;
            if (parentInterpreter != null)
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


        public IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode, TParent>> Children()
        {
            var container = (XContainer)(object)_node;
            var children = container.Nodes().OfType<TNode>();
            var intepreters = children.Select(child =>
            {
                var interpreter = new XNodeInterpreter<TNode, IXNodeInterpreter<TNode, TParent>>(child, this);
                return interpreter;
            }).ToArray();

            var iterator = new XNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode, TParent>>(intepreters, this);
            return iterator;
        }

        IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode, TParent>> IXNodeInterpreter<TNode, TParent>.Children()
        {
            return Children();
        }

        IXNodeInterpreterIterator<TNode, IXNodeInterpreter<TNode>> IXNodeInterpreter<TNode>.Children()
        {
            return Children();
        }

        IXNodeInterpreterIterator<XNode, IXNodeInterpreter> IXNodeInterpreter.Children()
        {
            return Children();
        }


        public IXNodeInterpreter<XNode, IXNodeInterpreter<TNode, TParent>> GetChild(int index)
        {
            //var iterator = Children();
            //var node = iterator.GetAtIndex(index).GetNode();
            //var navigator = new XNodeInterpreter<XNode, IXNodeInterpreter<TNode, TParent>>(node, this);
            //return navigator;

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
            var interpreter = GetChild(index);
            return interpreter;
        }

        IXNodeInterpreter<XNode, IXNodeInterpreter<XNode>> IXNodeInterpreter.GetChild(int index)
        {
            var interpreter = GetChild(index);
            return interpreter;
        }

        public TParent Return()
        {
            return _parent;
        }
    }
}
