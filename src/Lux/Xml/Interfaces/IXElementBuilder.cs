using System.Xml.Linq;
using Lux.Interfaces;

namespace Lux.Xml
{
    public interface IXElementBuilder
    {
        IXElementBuilder New();
        IXElementBuilder SetTagName(string name);
        IXElementBuilder SetAttribute(string name, object value);
        IXElementBuilder SetValue(object value);
        IXElementBuilder AppendChild(XNode node);

        XElement Create();
    }
    
    public interface IXElementBuilder<out TNode>
    {
        IXElementBuilder<TNode> New();
        IXElementBuilder<TNode> SetTagName(string name);
        IXElementBuilder<TNode> SetAttribute(string name, object value);
        IXElementBuilder<TNode> SetValue(object value);
        IXElementBuilder<TNode> AppendChild(XNode node);

        TNode Create();
    }

    public interface IFluentXElementBuilder<TNode, out TParent> : IFluentReturn<TParent>
    {
        IFluentXElementBuilder<TNode, TParent> New();
        IFluentXElementBuilder<TNode, TParent> SetTagName(string name);
        IFluentXElementBuilder<TNode, TParent> SetAttribute(string name, object value);
        IFluentXElementBuilder<TNode, TParent> SetValue(object value);
        IFluentXElementBuilder<TNode, IFluentXElementBuilder<TNode, TParent>> BuildChild();

        TParent Create();
    }
}
