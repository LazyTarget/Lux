using System.Collections.Generic;
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


            public void SetProperty(PropertyElement property)
            {
                Properties[property.PropertyName] = property;
            }
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


            public string PropertyName
            {
                get { return this.GetAttributeValue("name"); }
                set { this.SetAttributeValue("name", value); }
            }

            public string PropertyValue
            {
                get { return this.GetAttributeValue("value"); }
                set { this.SetAttributeValue("value", value); }
            }


            public static PropertyElement Create(string name, string value)
            {
                var prop = new PropertyElement();
                prop.PropertyName = name;
                prop.PropertyValue = value;
                return prop;
            }
        }



        [TestCase]
        public void CodeSyntax_XNodeInterpreter_BuildAndAppend()
        {
            var doc = new DocumentElement();
            doc.SetProperty(PropertyElement.Create("FirstName", "Peter"));
            doc.SetProperty(PropertyElement.Create("LastName", "Åslund"));

            //doc.CreateInterpreter().BuildAndAppend()..To<XElement>().GetChild(0).To<XElement>().
            //doc.CreateInterpreter().BuildAndAppend<>()
            


            Assert.IsNotNull(doc);
            Assert.AreEqual(DocumentElement.TAGNAME, doc.Name.ToString());
            Assert.AreEqual("1.0", doc.GetAttributeValue("version"));

            Assert.AreEqual(1, doc.Elements().Count());

            var propertyElement = doc.Elements().First();
            Assert.AreEqual(PropertyElement.TAGNAME, propertyElement.Name.ToString());
            Assert.AreEqual("FirstName", propertyElement.GetAttributeValue("name"));
            Assert.AreEqual("Peter", propertyElement.GetAttributeValue("value"));
        }

    }
}
