using Lux.Model;
using Lux.Serialization.Json.JsonNet;
using Newtonsoft.Json.Linq;
using NUnit.Framework;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public class JsonObjectModelTests : ObjectModelTestBase
    {
        protected override IObjectModel CreateObjectModel(object args)
        {
            var propertyParser = CreatePropertyParser();
            var element = SerializeWithPropertyParser(propertyParser, args);
            var objectModel = new JsonObjectModel(element, propertyParser);
            return objectModel;
        }

        protected virtual IJObjectPropertyParser CreatePropertyParser()
        {
            var propertyParser = new JObjectPropertyParser();
            return propertyParser;
        }

        protected virtual JObject SerializeWithPropertyParser(IJObjectPropertyParser propertyParser, object obj)
        {
            var result = new JObject();
            var mirrorObjectModel = new MirrorObjectModel(obj);
            var properties = mirrorObjectModel.GetProperties();
            foreach (var property in properties)
            {
                propertyParser.DefineProperty(result, property.Name, property.Type, property.Value, property.ReadOnly);
            }
            return result;
        }


    }
}
