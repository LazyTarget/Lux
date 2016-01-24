using System;
using System.Xml.Linq;

namespace Lux.Xml
{
    public class FluentXElementBuilder<TNode, TParent> : IFluentXElementBuilder<TNode, TParent>
        where TNode : XElement, new()
    {
        private readonly TParent _parent;
        private readonly IXElementBuilder<TNode> _builder;

        public FluentXElementBuilder(TParent parent)
        {
            _parent = parent;
            _builder = new XElementBuilder<TNode>();
        }

        public FluentXElementBuilder(TParent parent, IXElementBuilder<TNode> builder)
        {
            _parent = parent;
            _builder = builder;
        }

        public Action<TNode> OnCreate { get; set; }

        public TNode Result { get; private set; }


        public IFluentXElementBuilder<TNode, TParent> New()
        {
            //_builder.New();
            //return this;
            return new FluentXElementBuilder<TNode, TParent>(_parent, _builder.New());
        }

        public IFluentXElementBuilder<TNode, TParent> SetTagName(string name)
        {
            _builder.SetTagName(name);
            return this;
        }

        public IFluentXElementBuilder<TNode, TParent> SetAttribute(string name, object value)
        {
            _builder.SetAttribute(name, value);
            return this;
        }

        public IFluentXElementBuilder<TNode, TParent> SetValue(object value)
        {
            _builder.SetValue(value);
            return this;
        }

        public IFluentXElementBuilder<TNode, IFluentXElementBuilder<TNode, TParent>> BuildChild()
        {
            var builder = _builder.New();

            var childBuilder = new FluentXElementBuilder<TNode, IFluentXElementBuilder<TNode, TParent>>(this, builder);
            childBuilder.OnCreate = (result) =>
            {
                _builder.AppendChild(result);
            };
            return childBuilder;
        }

        public TParent Create()
        {
            Result = _builder.Create();
            if (OnCreate != null)
                OnCreate(Result);
            return _parent;
        }

        public TParent Return()
        {
            return _parent;
        }
    }
}
