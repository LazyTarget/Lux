namespace Lux.Serialization.Xml
{
    public interface IXmlSerializer
    {
        System.Xml.Linq.XDocument Serialize(IXmlDocument document);
        //System.Xml.Linq.XNode Serialize(IXmlNode node);

        IXmlNode Deserialize(System.Xml.Linq.XNode node);
        TNode Deserialize<TNode>(System.Xml.Linq.XNode node) where TNode : IXmlNode;
    }
}
