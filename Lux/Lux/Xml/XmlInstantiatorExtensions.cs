using System;

namespace Lux.Xml
{
    public static class XmlInstantiatorExtensions
    {
        private static readonly IXmlInstantiator XmlInstantiator = new XmlInstantiator();


        public static IXmlInstantiator Instantiator(this IXmlNode node)
        {
            return XmlInstantiator;
        }

        public static IXmlInstantiator Instantiator(this IXmlNode node, IXmlInstantiator instantiator)
        {
            return instantiator;
        }

        public static IXmlInstantiator Instantiator(this IXmlNode node, Func<XmlInstantiator> instantiator)
        {
            return instantiator();
        }



    }
}
