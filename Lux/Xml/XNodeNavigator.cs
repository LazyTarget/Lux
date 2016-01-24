using System;
using System.Linq;
using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public class XNodeNavigator
    {
        //public static XNodeNavigator<IXmlDocument> Create(IXmlDocument document)
        //{
        //    // method is simply a shortcut to XNodeNavigator<IXmlDocument>.Create(..)

        //    var navigator = XNodeNavigator<IXmlDocument>.Create(document);
        //    return navigator;
        //}

        //public static XNodeNavigator<TNode> Create<TNode>(TNode node)
        //    where TNode : IXmlNode
        //{
        //    // method is simply a shortcut to XNodeNavigator<IXmlDocument>.Create(..)

        //    var navigator = XNodeNavigator<TNode>.Create(node);
        //    return navigator;
        //}
    }
    


    public class XNodeNavigator<TNode, TParent> : IXNodeNavigator<TNode, TParent> //IFluentReturn<TParent>
        where TNode : XNode
    {
        protected TParent _parent;
        protected readonly TNode _node;
        protected XNodeNavigator<XContainer, TParent> _parentNavigator;
        
        /* Constructors */

        protected XNodeNavigator(XNodeNavigator<TNode, TParent> navigator)
            : this(navigator._node)
        {
            if (navigator == null)
                throw new ArgumentNullException(nameof(navigator));
            SetState(navigator);
        }

        protected XNodeNavigator(TNode node, TParent parent)
            : this(node)
        {
            _parent = parent;
        }

        protected XNodeNavigator(TNode node)
        {
            if (node == null)
                throw new ArgumentNullException(nameof(node));
            _node = node;
        }


        private void SetState(XNodeNavigator<TNode, TParent> state)
        {
            //_node = state._node;
            _parentNavigator = state._parentNavigator;
            _parent = state._parent;
        }


        public static IXNodeNavigator<XDocument> Create(XObject node)
        {
            var document = node.Document;

            var navigator = new XNodeNavigator<XDocument, TParent>(document);
            return navigator;
        }

        internal static IXNodeNavigator<TNode> Create(TNode node, XNodeNavigator<XContainer, TParent> parentNavigator = null)
        {
            var navigator = new XNodeNavigator<TNode, TParent>(node);
            navigator._parentNavigator = parentNavigator;
            navigator._parent = parentNavigator._parent;
            return navigator;
        }



        /* Traversing */

        public TNode GetNode()
        {
            return _node;
        }

        XNode IXNodeNavigator.GetNode()
        {
            return GetNode();
        }

        public IXNodeNavigator<TNodeType, IXNodeNavigator<TNode, TParent>> To<TNodeType>()
            where TNodeType : XNode
        {
            var node = (TNodeType) (object) _node;
            var navigator = new XNodeNavigator<TNodeType, IXNodeNavigator<TNode, TParent>>(node, this);

            //navigator._parent = _parentNavigator;      // important to 'restore' all state from previous object (node, parent, etc.)
            return navigator;
        }

        IXNodeNavigator<TNodeType, IXNodeNavigator<TNode>> IXNodeNavigator<TNode>.To<TNodeType>()
        {
            var navigator = To<TNodeType>();
            return navigator;
        }

        IXNodeNavigator<TNodeType, IXNodeNavigator<XNode>> IXNodeNavigator.To<TNodeType>()
        {
            var navigator = To<TNodeType>();
            return navigator;
        }


        public IXNodeNavigator<XNode, IXNodeNavigator<TNode, TParent>> GetChild(int index)
        {
            var container = (XContainer) (object) _node;
            var child = container.Nodes().ElementAt(index);

            var navigator = new XNodeNavigator<XNode, IXNodeNavigator<TNode, TParent>>(child, this);
            //navigator._parentNavigator = _parentNavigator;
            //navigator._parent = this;
            //navigator._parent = this.To<XContainer>();
            return navigator;
        }

        IXNodeNavigator<XNode, IXNodeNavigator<TNode>> IXNodeNavigator<TNode>.GetChild(int index)
        {
            var navigator = GetChild(index);
            return navigator;
        }

        IXNodeNavigator<XNode, IXNodeNavigator<XNode>> IXNodeNavigator.GetChild(int index)
        {
            var navigator = GetChild(index);
            return navigator;
        }

        //public XNodeNavigator<XNode> GetChild(int index)
        //{
        //    var container = (XContainer) (object) _node;
        //    var children = container.Nodes().ToList();
        //    //Assert.IsFalse(index < 0 || index > children.Count, "Child index is out of range");

        //    var node = children.ElementAt(index);
        //    var navigator = new XNodeNavigator<XNode>(node);
        //    navigator._parent = this.To<XContainer>();
        //    return navigator;
        //}

        //public XNodeNavigator<TTagType> GetChildAs<TTagType>(int index)
        //    where TTagType : XNode
        //{
        //    var navigator = GetChild(index);
        //    var result = navigator.To<TTagType>();
        //    return result;
        //}

        //public XNodeNavigator<XContainer> GetParent()
        //{
        //    //Assert.IsNotNull(_parent, "No parent");
        //    // todo: Good addition would be to return the same TTagType type as the parent, if IsTagType() was called earlier
        //    return _parentNavigator;
        //}

        //public XNodeNavigator<TTagType> GetParent<TTagType>()
        //    where TTagType : XNode
        //{
        //    //Assert.IsNotNull(_parent, "No parent");
        //    var navigator = _parentNavigator.To<TTagType>();
        //    return navigator;
        //}

        public TParent Return()
        {
            return _parent;
        }



        /* Assertions */

        //public XNodeNavigator<TTagType> To<TTagType>()
        //    where TTagType : XNode
        //{
        //    //Assert.IsInstanceOfType(_node, typeof(TTagType));
        //    //return this;
            
        //    //var typedTag = _node as TTagType;
        //    var typedTag = (TTagType) (object) _node;
        //    var navigator = new XNodeNavigator<TTagType>(typedTag);
        //    navigator._parent = _parentNavigator;      // important to 'restore' all state from previous object (node, parent, etc.)
        //    return navigator;
        //}


    }


    /* Custom */


    //public class VxmlSwitchStatementAssertionHelper : VxmlAssertionHelper<IfTagBase>
    //{
    //    protected readonly TellusVoice.Designer.Models.ConnectionType _connType;
    //    protected readonly VxmlAssertionHelper<If> _switchAssertion;

    //    private VxmlSwitchStatementAssertionHelper(IfTagBase statementTag, VxmlAssertionHelper<If> switchAssertionHelper, TellusVoice.Designer.Models.ConnectionType connType)
    //        : base(statementTag)
    //    {
    //        if (switchAssertionHelper == null)
    //            throw new ArgumentNullException("navigator");

    //        _parent = switchAssertionHelper.IsTagType<BaseTag>();

    //        _switchAssertion = switchAssertionHelper;
    //        _connType = connType;
    //    }


    //    public static VxmlSwitchStatementAssertionHelper Create(VxmlAssertionHelper<If> navigator, TellusVoice.Designer.Models.ConnectionType connType)
    //    {
    //        var statementNum = TellusVoice.Designer.Models.Extensions.GetSwitchNumber(connType);
    //        if (statementNum < 0)
    //            throw new InvalidOperationException("Invalid connection type for switch statement ('" + connType + "')");

    //        If ifTag = navigator.GetTag();
    //        IfTagBase tag = null;
    //        if (connType == Models.ConnectionType.switchStatement1)
    //        {
    //            tag = ifTag;
    //        }
    //        else if (connType == Models.ConnectionType.switchStatementElse)
    //        {
    //            tag = (Else)ifTag.Children.LastOrDefault(x => typeof(Else) == x.GetType());
    //        }
    //        else
    //        {
    //            var x = 1;
    //            for (var i = 0; i < ifTag.Children.Count; i++)
    //            {
    //                var c = ifTag.Children.ElementAt(i);
    //                if (c.GetType() == typeof(Elseif))
    //                {
    //                    ++x;
    //                    if (statementNum == x)
    //                    {
    //                        tag = (Elseif)c;
    //                        break;
    //                    }
    //                }
    //            }
    //        }

    //        var a = new VxmlSwitchStatementAssertionHelper(tag, navigator, connType);
    //        return a;
    //    }



    //    /* Traversing */

    //    public XNodeNavigator<If> Switch()
    //    {
    //        return _switchAssertion;
    //    }


    //    /* Assertions */

    //    public VxmlSwitchStatementAssertionHelper WithCondition(string condition)
    //    {
    //        var ifTag = _tag as If;
    //        var elseIfTag = _tag as Elseif;
    //        var elseTag = _tag as Else;

    //        VxmlAssertionHelperExtensions.WithCondition(this, condition);

    //        if (ifTag != null)
    //            Assert.AreEqual(condition, ifTag.cond);
    //        else if (elseIfTag != null)
    //            Assert.AreEqual(condition, elseIfTag.cond);
    //        else if (elseTag != null && condition != null)
    //            throw new InvalidOperationException("Else statements cannot have conditions");
    //        return this;
    //    }

    //    public VxmlSwitchStatementAssertionHelper WithTarget(TellusVoice.Designer.Models.ScriptBlock block)
    //    {
    //        //throw new NotImplementedException();
    //        return this;
    //    }

    //}


}
