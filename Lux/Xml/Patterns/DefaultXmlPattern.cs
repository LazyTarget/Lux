namespace Lux.Xml
{
    public class DefaultXmlPattern : XmlPattern
    {
        public DefaultXmlPattern()
        {
            Conventions.Clear();
            Conventions.Add(new DefaultObjectXmlConvention());
            Conventions.Add(new DefaultArrayXmlConvention());
        }

    }
}
