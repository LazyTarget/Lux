using System;
using System.Linq;
using System.Xml.Linq;
using Lux.Model;
using Lux.Model.Xml;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public class XmlObjectModelTests : ObjectModelTestBase
    {
        protected override IObjectModel CreateObjectModel(object args)
        {
            var propertyParser = CreatePropertyParser();
            var element = SerializeWithPropertyParser(propertyParser, args);
            var objectModel = new XmlObjectModel(element, propertyParser);
            return objectModel;
        }

        protected virtual IXmlObjectModelPropertyParser CreatePropertyParser()
        {
            var propertyParser = new XElementPropertyParser();
            return propertyParser;
        }

        protected virtual XElement SerializeWithPropertyParser(IXmlObjectModelPropertyParser propertyParser, object obj)
        {
            var element = new XElement("obj");
            var mirrorObjectModel = new MirrorObjectModel(obj);
            var properties = mirrorObjectModel.GetProperties();
            foreach (var property in properties)
            {
                propertyParser.DefineProperty(element, property.Name, property.Type, property.Value, property.ReadOnly);
            }
            return element;
        }


    }
}
