using System;

namespace Lux.Config
{
    public class XmlConfigLocation : ConfigLocationBase, IXmlConfigLocation
    {
        public Uri Uri { get; set; }
        public string RootElementName { get; set; }
        public string RootElementExpression { get; set; }
    }
}