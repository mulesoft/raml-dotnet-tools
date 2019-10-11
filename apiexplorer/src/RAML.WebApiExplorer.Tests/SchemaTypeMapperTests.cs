using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RAML.WebApiExplorer.Tests
{
    [TestClass]
    public class SchemaTypeMapperTests
    {

		[TestMethod]
		public void ShouldConvertToInteger()
		{
			Assert.AreEqual("integer", SchemaTypeMapper.Map(typeof(int)));
		}

		[TestMethod]
		public void ShouldConvertToString()
		{
			Assert.AreEqual("string", SchemaTypeMapper.Map(typeof(string)));
		}

		[TestMethod]
		public void ShouldConvertToBoolean()
		{
			Assert.AreEqual("boolean", SchemaTypeMapper.Map(typeof(bool)));
		}

		[TestMethod]
		public void ShouldConvertToNumber_WhenDecimal()
		{
			Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(decimal)));
		}

        [TestMethod]
        public void ShouldConvertToNumber_WhenFloat()
        {
            Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(float)));
        }

        [TestMethod]
        public void ShouldConvertToString_WhenDateTime()
        {
            Assert.AreEqual("string", SchemaTypeMapper.Map(typeof(DateTime)));
        }


        [TestMethod]
        public void ShouldConvertToInteger_WhenNullable()
        {
            Assert.AreEqual("integer", SchemaTypeMapper.Map(typeof(int?)));
        }

        [TestMethod]
        public void ShouldConvertToBoolean_WhenNullable()
        {
            Assert.AreEqual("boolean", SchemaTypeMapper.Map(typeof(bool?)));
        }

        [TestMethod]
        public void ShouldConvertToNumber_WhenNullableDecimal()
        {
            Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(decimal?)));
        }

        [TestMethod]
        public void ShouldConvertToNumber_WhenNullableFloat()
        {
            Assert.AreEqual("number", SchemaTypeMapper.Map(typeof(float?)));
        }

        [TestMethod]
        public void ShouldConvertToString_WhenNullableDateTime()
        {
            Assert.AreEqual("string", SchemaTypeMapper.Map(typeof(DateTime?)));
        }



        [TestMethod]
        public void ShouldGetAttributeInteger()
        {
            Assert.AreEqual("\"integer\"", SchemaTypeMapper.GetAttribute(typeof(int)));
        }

        [TestMethod]
        public void ShouldGetAttributeString()
        {
            Assert.AreEqual("\"string\"", SchemaTypeMapper.GetAttribute(typeof(string)));
        }

        [TestMethod]
        public void ShouldGetAttributeBoolean()
        {
            Assert.AreEqual("\"boolean\"", SchemaTypeMapper.GetAttribute(typeof(bool)));
        }

        [TestMethod]
        public void ShouldGetAttributeNumber_WhenDecimal()
        {
            Assert.AreEqual("\"number\"", SchemaTypeMapper.GetAttribute(typeof(decimal)));
        }

        [TestMethod]
        public void ShouldGetAttributeNumber_WhenFloat()
        {
            Assert.AreEqual("\"number\"", SchemaTypeMapper.GetAttribute(typeof(float)));
        }

        [TestMethod]
        public void ShouldGetAttributeString_WhenDateTime()
        {
            Assert.AreEqual("\"string\"", SchemaTypeMapper.GetAttribute(typeof(DateTime)));
        }


        [TestMethod]
        public void ShouldGetAttributeInteger_WhenNullable()
        {
            Assert.AreEqual("[\"integer\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(int?)));
        }

        [TestMethod]
        public void ShouldGetAttributeBoolean_WhenNullable()
        {
            Assert.AreEqual("[\"boolean\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(bool?)));
        }

        [TestMethod]
        public void ShouldGetAttributeNumber_WhenNullableDecimal()
        {
            Assert.AreEqual("[\"number\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(decimal?)));
        }

        [TestMethod]
        public void ShouldGetAttributeNumber_WhenNullableFloat()
        {
            Assert.AreEqual("[\"number\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(float?)));
        }

        [TestMethod]
        public void ShouldGetAttributeString_WhenNullableDateTime()
        {
            Assert.AreEqual("\"string\"", SchemaTypeMapper.GetAttribute(typeof(DateTime?)));
        }

    }
}