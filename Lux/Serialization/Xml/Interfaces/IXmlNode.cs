namespace Lux.Serialization.Xml
{
    public interface IXmlNode : INode, IXmlConfigurable, IXmlExportable
    {
        IXmlPattern Pattern { get; }
        IXmlNode ParentNode { get; }
    }
}