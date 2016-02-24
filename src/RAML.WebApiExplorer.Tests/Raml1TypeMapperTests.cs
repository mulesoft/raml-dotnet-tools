using NUnit.Framework;
using System;

namespace RAML.WebApiExplorer.Tests
{
    [TestFixture]
    public class Raml1TypeMapperTests
    {

		[Test]
		public void ShouldConvertToInteger()
		{
			Assert.AreEqual("integer", Raml1TypeMapper.Map(typeof(int)));
		}

		[Test]
		public void ShouldConvertToString()
		{
			Assert.AreEqual("string", Raml1TypeMapper.Map(typeof(string)));
		}

		[Test]
		public void ShouldConvertToBoolean()
		{
			Assert.AreEqual("boolean", Raml1TypeMapper.Map(typeof(bool)));
		}

		[Test]
		public void ShouldConvertToNumber_WhenDecimal()
		{
			Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(decimal)));
		}

        [Test]
        public void ShouldConvertToNumber_WhenFloat()
        {
            Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(float)));
        }

        [Test]
        public void ShouldConvertToString_WhenDateTime()
        {
            Assert.AreEqual("date", Raml1TypeMapper.Map(typeof(DateTime)));
        }

        [Test]
        public void ShouldConvertToFile_WhenByteArray()
        {
            Assert.AreEqual("file", Raml1TypeMapper.Map(typeof(byte[])));
        }

        [Test]
        public void ShouldConvertToInteger_WhenNullable()
        {
            Assert.AreEqual("integer", Raml1TypeMapper.Map(typeof(int?)));
        }

        [Test]
        public void ShouldConvertToBoolean_WhenNullable()
        {
            Assert.AreEqual("boolean", Raml1TypeMapper.Map(typeof(bool?)));
        }

        [Test]
        public void ShouldConvertToNumber_WhenNullableDecimal()
        {
            Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(decimal?)));
        }

        [Test]
        public void ShouldConvertToNumber_WhenNullableFloat()
        {
            Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(float?)));
        }

        [Test]
        public void ShouldConvertToString_WhenNullableDateTime()
        {
            Assert.AreEqual("date", Raml1TypeMapper.Map(typeof(DateTime?)));
        }

    }
}