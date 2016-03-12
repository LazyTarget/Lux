using System;
using Lux.Model;
using NUnit.Framework;

namespace Lux.Tests.Model.PropertyTests
{
    [TestFixture]
    public class PropertyTests : PropertyTestBase
    {
        protected override IProperty CreateProperty(string name, Type type = null, object value = null, bool isReadOnly = false)
        {
            var property = new Property(name, type, isReadOnly, value);
            return property;
        }

    }
}
