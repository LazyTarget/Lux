using System;
using System.Reflection;
using Lux.Interfaces;
using Lux.Tests.Model.Models;
using NUnit.Framework;
using Ploeh.AutoFixture;
using Ploeh.AutoFixture.Kernel;

namespace Lux.Tests.Convert
{
    [TestFixture]
    public class ConverterTests : TestBase
    {
        protected virtual IConverter GetSUT()
        {
            return new Converter();
        }


        [TestCase]
        public virtual void CanConvertStringToEnum()
        {
            var expected = StringComparison.InvariantCultureIgnoreCase;
            var targetType = expected.GetType();
            var input = expected.ToString();

            var sut = GetSUT();
            var actual = sut.Convert(input, targetType);

            Assert.IsInstanceOf(targetType, actual);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public virtual void CanConvertIntToEnum()
        {
            var expected = StringComparison.InvariantCultureIgnoreCase;
            var targetType = expected.GetType();
            var input = (int)expected;

            var sut = GetSUT();
            var actual = sut.Convert(input, targetType);

            Assert.IsInstanceOf(targetType, actual);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public virtual void CanConvertStringToFlagsEnum()
        {
            var expected = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;
            var targetType = expected.GetType();
            var input = expected.ToString();

            var sut = GetSUT();
            var actual = sut.Convert(input, targetType);

            Assert.IsInstanceOf(targetType, actual);
            Assert.AreEqual(expected, actual);
        }


        [TestCase]
        public virtual void CanConvertIntToFlagsEnum()
        {
            var expected = BindingFlags.Instance | BindingFlags.Public | BindingFlags.GetProperty;
            var targetType = expected.GetType();
            var input = (int)expected;

            var sut = GetSUT();
            var actual = sut.Convert(input, targetType);

            Assert.IsInstanceOf(targetType, actual);
            Assert.AreEqual(expected, actual);
        }
        
    }
}
