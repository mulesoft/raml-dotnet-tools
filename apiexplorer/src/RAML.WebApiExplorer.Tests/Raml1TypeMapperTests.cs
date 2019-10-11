using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace RAML.WebApiExplorer.Tests
{
    [TestClass]
    public class Raml1TypeMapperTests
    {

		[TestMethod]
		public void ShouldConvertToInteger()
		{
			Assert.AreEqual("integer", Raml1TypeMapper.Map(typeof(int)));
		}

		[TestMethod]
		public void ShouldConvertToString()
		{
			Assert.AreEqual("string", Raml1TypeMapper.Map(typeof(string)));
		}

		[TestMethod]
		public void ShouldConvertToBoolean()
		{
			Assert.AreEqual("boolean", Raml1TypeMapper.Map(typeof(bool)));
		}

		[TestMethod]
		public void ShouldConvertToNumber_WhenDecimal()
		{
			Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(decimal)));
		}

        [TestMethod]
        public void ShouldConvertToNumber_WhenFloat()
        {
            Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(float)));
        }

        [TestMethod]
        public void ShouldConvertToString_WhenDateTime()
        {
            Assert.AreEqual("date", Raml1TypeMapper.Map(typeof(DateTime)));
        }

        [TestMethod]
        public void ShouldConvertToFile_WhenByteArray()
        {
            Assert.AreEqual("file", Raml1TypeMapper.Map(typeof(byte[])));
        }

        [TestMethod]
        public void ShouldConvertToInteger_WhenNullable()
        {
            Assert.AreEqual("integer", Raml1TypeMapper.Map(typeof(int?)));
        }

        [TestMethod]
        public void ShouldConvertToBoolean_WhenNullable()
        {
            Assert.AreEqual("boolean", Raml1TypeMapper.Map(typeof(bool?)));
        }

        [TestMethod]
        public void ShouldConvertToNumber_WhenNullableDecimal()
        {
            Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(decimal?)));
        }

        [TestMethod]
        public void ShouldConvertToNumber_WhenNullableFloat()
        {
            Assert.AreEqual("number", Raml1TypeMapper.Map(typeof(float?)));
        }

        [TestMethod]
        public void ShouldConvertToString_WhenNullableDateTime()
        {
            Assert.AreEqual("date", Raml1TypeMapper.Map(typeof(DateTime?)));
        }

    }
}