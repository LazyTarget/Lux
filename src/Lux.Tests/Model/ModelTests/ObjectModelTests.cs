using System;
using System.Linq;
using Lux.Model;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;

namespace Lux.Tests.Model.ModelTests
{
    [TestFixture]
    public class ObjectModelTests : ObjectModelTestBase
    {
        protected override IObjectModel CreateObjectModel(object args)
        {
            var objectModel = new ObjectModel();
            if (args != null)
            {
                var type = args.GetType();
                var properties = type.GetProperties();
                foreach (var propertyInfo in properties)
                {
                    var value = propertyInfo.GetValue(args);
                    var property = objectModel.DefineProperty(propertyInfo.Name, propertyInfo.PropertyType, value, !propertyInfo.CanWrite);
                }
            }
            return objectModel;
        }

    }
}
