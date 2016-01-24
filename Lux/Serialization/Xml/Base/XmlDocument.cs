using System;
using System.Xml.Linq;
using Lux.Xml;

namespace Lux.Serialization.Xml
{
    public class XmlDocument : XmlObject, IXmlDocument
    {
        private string _rootElementName;

        public XmlDocument()
            : this(XmlPattern.Default)
        {

        }

        public XmlDocument(IXmlPattern pattern)
            : base(null, pattern)
        {
            RootElementName = "Document";
        }


        public string RootElementName
        {
            get { return _rootElementName; }
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));
                _rootElementName = value;
            }
        }


        public void Configure(XDocument document)
        {
            var rootElement = GetRootElem(document, RootElementName);
            if (rootElement != null)
                Configure(rootElement);
        }

        public override void Configure(XElement element)
        {
            base.Configure(element);
        }


        public void Export(XDocument document)
        {
            var rootElement = GetOrCreateRootElem(document, RootElementName);
            Export(rootElement);
        }

        public override void Export(XElement element)
        {
            base.Export(element);
        }


        private XElement GetRootElem(XDocument document, string rootElementName)
        {
            var rootElement = document.Element(rootElementName);
            if (rootElement == null)
            {
                if (document.Root != null)
                    rootElement = document.Root.Element(rootElementName);
                else
                    rootElement = document.Element(rootElementName);
            }
            return rootElement;
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
