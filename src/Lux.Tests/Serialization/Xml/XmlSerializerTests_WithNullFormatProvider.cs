using System;
using System.Globalization;
using Lux.Serialization;
using Lux.Serialization.Xml;
using NUnit.Framework;

namespace Lux.Tests.Serialization.Xml
{
    [TestFixture]
    public class XmlSerializerTests_WithNullFormatProvider : SerializerTestBase
    {
        protected override ISerializer GetSUT()
        {
            return new XmlSerializer
            {
                Culture = GetCultureInfo(),
            };
        }

        protected override CultureInfo GetCultureInfo()
        {
            return null;
        }


        [TestCase]
        public override void SerializePoco()
        {
            base.SerializePoco();
        }

        [TestCase]
        public override void SerializePoco_WithNesting()
        {
            base.SerializePoco_WithNesting();
        }
    }
}
