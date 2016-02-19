using System;

namespace Lux.Serialization.Xml
{
    public static class XmlInstantiatorExtensions
    {
        private static readonly IXmlInstantiator XmlInstantiator = new CustomXmlSerializer();


        public static IXmlInstantiator Instantiator(this IXmlNode node)
        {
            return XmlInstantiator;
        }

        public static IXmlInstantiator Instantiator(this IXmlNode node, IXmlInstantiator instantiator)
        {
            return instantiator;
        }

        public static IXmlInstantiator Instantiator(this IXmlNode node, Func<CustomXmlSerializer> instantiator)
        {
            return instantiator();
        }



    }
}
