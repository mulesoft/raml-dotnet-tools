using NUnit.Framework;
using System;

namespace RAML.WebApiExplorer.Tests
{
    [TestFixture]
    public class SchemaTypeMapperTests
    {

		[Test]
		public void ShouldConvertToInteger()
		{
			Assert.AreEqual("integer", SchemaTypeMapper.Map(typeof(int)));
		}

		[Test]
		public void ShouldConvertToString()
		{
			Assert.AreEqual("string", SchemaTypeMapper.Map(typeof(string)));
		}

		[Test]
		public void ShouldConvertToBoolean()
		{
			Assert.AreEqual("boolean", SchemaTypeMapper.Map(typeof(bool)));
		}

		[Test]
		public void ShouldConvertToNumber_WhenDecimal()
		{
			Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(decimal)));
		}

        [Test]
        public void ShouldConvertToNumber_WhenFloat()
        {
            Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(float)));
        }

        [Test]
        public void ShouldConvertToString_WhenDateTime()
        {
            Assert.AreEqual("string", SchemaTypeMapper.Map(typeof(DateTime)));
        }


        [Test]
        public void ShouldConvertToInteger_WhenNullable()
        {
            Assert.AreEqual("integer", SchemaTypeMapper.Map(typeof(int?)));
        }

        [Test]
        public void ShouldConvertToBoolean_WhenNullable()
        {
            Assert.AreEqual("boolean", SchemaTypeMapper.Map(typeof(bool?)));
        }

        [Test]
        public void ShouldConvertToNumber_WhenNullableDecimal()
        {
            Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(decimal?)));
        }

        [Test]
        public void ShouldConvertToNumber_WhenNullableFloat()
        {
            Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(float?)));
        }

        [Test]
        public void ShouldConvertToString_WhenNullableDateTime()
        {
            Assert.AreEqual("string", SchemaTypeMapper.Map(typeof(DateTime?)));
        }



        [Test]
        public void ShouldGetAttributeInteger()
        {
            Assert.AreEqual("\"integer\"", SchemaTypeMapper.GetAttribute(typeof(int)));
        }

        [Test]
        public void ShouldGetAttributeString()
        {
            Assert.AreEqual("\"string\"", SchemaTypeMapper.GetAttribute(typeof(string)));
        }

        [Test]
        public void ShouldGetAttributeBoolean()
        {
            Assert.AreEqual("\"boolean\"", SchemaTypeMapper.GetAttribute(typeof(bool)));
        }

        [Test]
        public void ShouldGetAttributeNumber_WhenDecimal()
        {
            Assert.AreEqual("\"number\"", SchemaTypeMapper.GetAttribute(typeof(decimal)));
        }

        [Test]
        public void ShouldGetAttributeNumber_WhenFloat()
        {
            Assert.AreEqual("\"number\"", SchemaTypeMapper.GetAttribute(typeof(float)));
        }

        [Test]
        public void ShouldGetAttributeString_WhenDateTime()
        {
            Assert.AreEqual("\"string\"", SchemaTypeMapper.GetAttribute(typeof(DateTime)));
        }


        [Test]
        public void ShouldGetAttributeInteger_WhenNullable()
        {
            Assert.AreEqual("[\"integer\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(int?)));
        }

        [Test]
        public void ShouldGetAttributeBoolean_WhenNullable()
        {
            Assert.AreEqual("[\"boolean\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(bool?)));
        }

        [Test]
        public void ShouldGetAttributeNumber_WhenNullableDecimal()
        {
            Assert.AreEqual("[\"number\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(decimal?)));
        }

        [Test]
        public void ShouldGetAttributeNumber_WhenNullableFloat()
        {
            Assert.AreEqual("[\"number\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(float?)));
        }

        [Test]
        public void ShouldGetAttributeString_WhenNullableDateTime()
        {
            Assert.AreEqual("\"string\"", SchemaTypeMapper.GetAttribute(typeof(DateTime?)));
        }

    }
}