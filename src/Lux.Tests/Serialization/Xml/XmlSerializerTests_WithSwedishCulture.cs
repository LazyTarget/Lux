﻿using System;
using System.Globalization;
using Lux.Serialization;
using Lux.Serialization.Xml;
using NUnit.Framework;

namespace Lux.Tests.Serialization.Xml
{
    [TestFixture]
    public class XmlSerializerTests_WithSwedishCulture : SerializerTestBase
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
            return CultureInfo.GetCultureInfo("sv-SE");
        }


        [TestCase]
        public override void SerializePoco()
        {
            base.SerializePoco();
        }
    }
}
