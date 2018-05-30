using System;
using AMF.WebApiExplorer;
using Xunit;

namespace AMF.NetCoreApiExplorer.Tests
{
    public class Raml1TypeMapperTests
    {

		[Fact]
		public void ShouldConvertToInteger()
		{
			Assert.Equal("integer", Raml1TypeMapper.Map(typeof(int)));
		}

		[Fact]
		public void ShouldConvertToString()
		{
			Assert.Equal("string", Raml1TypeMapper.Map(typeof(string)));
		}

		[Fact]
		public void ShouldConvertToBoolean()
		{
			Assert.Equal("boolean", Raml1TypeMapper.Map(typeof(bool)));
		}

		[Fact]
		public void ShouldConvertToNumber_WhenDecimal()
		{
			Assert.Equal("number", Raml1TypeMapper.Map(typeof(decimal)));
		}

        [Fact]
        public void ShouldConvertToNumber_WhenFloat()
        {
            Assert.Equal("number", Raml1TypeMapper.Map(typeof(float)));
        }

        [Fact]
        public void ShouldConvertToDatetime_WhenDateTime()
        {
            Assert.Equal("datetime", Raml1TypeMapper.Map(typeof(DateTime)));
        }

        [Fact]
        public void ShouldConvertToFile_WhenByteArray()
        {
            Assert.Equal("file", Raml1TypeMapper.Map(typeof(byte[])));
        }

        [Fact]
        public void ShouldConvertToInteger_WhenNullable()
        {
            Assert.Equal("integer", Raml1TypeMapper.Map(typeof(int?)));
        }

        [Fact]
        public void ShouldConvertToBoolean_WhenNullable()
        {
            Assert.Equal("boolean", Raml1TypeMapper.Map(typeof(bool?)));
        }

        [Fact]
        public void ShouldConvertToNumber_WhenNullableDecimal()
        {
            Assert.Equal("number", Raml1TypeMapper.Map(typeof(decimal?)));
        }

        [Fact]
        public void ShouldConvertToNumber_WhenNullableFloat()
        {
            Assert.Equal("number", Raml1TypeMapper.Map(typeof(float?)));
        }

        [Fact]
        public void ShouldConvertToDatetime_WhenNullableDateTime()
        {
            Assert.Equal("datetime", Raml1TypeMapper.Map(typeof(DateTime?)));
        }

    }
}