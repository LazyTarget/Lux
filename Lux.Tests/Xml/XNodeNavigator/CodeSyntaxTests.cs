﻿using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Lux.Xml;
using NUnit.Framework;

namespace Lux.Tests.Xml.XNodeNavigator
{
    [TestFixture]
    public class CodeSyntaxTests : TestBase
    {
        public class DocumentElement : XElement
        {
            public const string TAGNAME = "document";

            public DocumentElement()
                : this(TAGNAME)
            {

            }

            public DocumentElement(XName name) : base(name)
            {
            }

            public DocumentElement(XName name, object content) : base(name, content)
            {
            }

            public DocumentElement(XName name, params object[] content) : base(name, content)
            {
            }

            public DocumentElement(XElement other) : base(other)
            {
            }

            public DocumentElement(XStreamingElement other) : base(other)
            {
            }


            //public IEnumerable<PropertyElement> Properties { get { return Elements().OfType<PropertyElement>(); } }
            public IDictionary<string, PropertyElement> Properties { get { return Elements().OfType<PropertyElement>().ToDictionary(x => x.PropertyName, x => x); } }
        }

        public class PropertyElement : XElement
        {
            public const string TAGNAME = "property";

            public PropertyElement()
                : this(TAGNAME)
            {

            }

            public PropertyElement(XName name) : base(name)
            {
            }

            public PropertyElement(XName name, object content) : base(name, content)
            {
            }

            public PropertyElement(XName name, params object[] content) : base(name, content)
            {
            }

            public PropertyElement(XElement other) : base(other)
            {
            }

            public PropertyElement(XStreamingElement other) : base(other)
            {
            }


            public string PropertyName => this.GetAttributeValue("name");

            public string PropertyValue => this.GetAttributeValue("value");
        }

        
        
        //[TestCase]
        //public void CodeSyntax_XElementBuilder()
        //{
        //    XNode x = new XElement("");
        //    XPathNavigator nav = x.CreateNavigator();

        //    IXNodeNavigator mav;

        //    var builder = new Lux.Xml.XElementBuilder();
        //    var doc = builder
        //        .SetTagName(DocumentElement.TAGNAME)
        //        .SetAttribute("version", "1.0")
        //        .AppendChild(
        //            new Lux.Xml.XElementBuilder()
        //                .SetTagName(PropertyElement.TAGNAME)
        //                .SetAttribute("name", "FirstName")
        //                .SetAttribute("value", "Peter")
        //                .Create()
        //        )
        //        .Create();

        //    Assert.IsNotNull(doc);
        //    Assert.AreEqual(DocumentElement.TAGNAME, doc.Name.ToString());
        //    Assert.AreEqual("1.0", doc.GetAttributeValue("version"));

        //    Assert.AreEqual(1, doc.Elements().Count());

        //    var propertyElement = doc.Elements().First();
        //    Assert.AreEqual(PropertyElement.TAGNAME, propertyElement.Name.ToString());
        //    Assert.AreEqual("FirstName", propertyElement.GetAttributeValue("name"));
        //    Assert.AreEqual("Peter", propertyElement.GetAttributeValue("value"));
        //}

    }
}
