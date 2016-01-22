using System;
using System.Xml.Linq;

namespace Lux.Serialization.Xml
{
    public abstract class XmlDocument : XmlNode, IXmlDocument
    {
        protected XmlDocument()
            : this(XmlPattern.Instance)
        {

        }

        private XmlDocument(IXmlPattern pattern)
            : base(pattern)
        {
            RootElementName = "Document";
        }

        public string RootElementName { get; set; }


        public override void Configure(XElement element)
        {
            base.Configure(element);
        }

        public override void Export(XElement element)
        {
            base.Export(element);
        }

        public void Export(XDocument document)
        {
            var rootElement = GetOrCreateRootElem(document, RootElementName);
            Export(rootElement);
        }
        

        private XElement GetOrCreateRootElem(XDocument document, string rootElementName)
        {
            var rootElement = document.Element(rootElementName);
            if (rootElement == null)
            {
                if (document.Root != null)
                    rootElement = document.Root.GetOrCreateElement(rootElementName);
                else
                    rootElement = document.GetOrCreateElement(rootElementName);
            }
            return rootElement;
        }

    }
}
