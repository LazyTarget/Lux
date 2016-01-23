using System;

namespace Lux.Serialization.Xml
{
    public static class XmlInstantiatorExtensions
    {
        private static readonly IXmlInstantiator XmlInstantiator = new XmlSerializer();


        public static IXmlInstantiator Instantiator(this IXmlNode node)
        {
            return XmlInstantiator;
        }

        public static IXmlInstantiator Instantiator(this IXmlNode node, IXmlInstantiator instantiator)
        {
            return instantiator;
        }

        public static IXmlInstantiator Instantiator(this IXmlNode node, Func<XmlSerializer> instantiator)
        {
            return instantiator();
        }



    }
}
