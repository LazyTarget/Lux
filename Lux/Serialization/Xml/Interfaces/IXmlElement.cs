namespace Lux.Serialization.Xml
{
    public interface IXmlElement : IXmlNode, IXmlNodeContainer
    {
        string TagName { get; set; }
    }
}
