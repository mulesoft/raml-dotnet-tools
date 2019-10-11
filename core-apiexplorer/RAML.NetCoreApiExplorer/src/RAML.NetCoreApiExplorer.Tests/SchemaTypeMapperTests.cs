using System;
using RAML.WebApiExplorer;
using Xunit;

namespace RAML.NetCoreApiExplorer.Tests
{
    public class SchemaTypeMapperTests
    {

		[Fact]
		public void ShouldConvertToInteger()
		{
			Assert.Equal("integer", SchemaTypeMapper.Map(typeof(int)));
		}

		[Fact]
		public void ShouldConvertToString()
		{
			Assert.Equal("string", SchemaTypeMapper.Map(typeof(string)));
		}

		[Fact]
		public void ShouldConvertToBoolean()
		{
			Assert.Equal("boolean", SchemaTypeMapper.Map(typeof(bool)));
		}

		[Fact]
		public void ShouldConvertToNumber_WhenDecimal()
		{
			Assert.Equal("number", SchemaTypeMapper.Map(typeof(decimal)));
		}

        [Fact]
        public void ShouldConvertToNumber_WhenFloat()
        {
            Assert.Equal("number", SchemaTypeMapper.Map(typeof(float)));
        }

        [Fact]
        public void ShouldConvertToString_WhenDateTime()
        {
            Assert.Equal("string", SchemaTypeMapper.Map(typeof(DateTime)));
        }


        [Fact]
        public void ShouldConvertToInteger_WhenNullable()
        {
            Assert.Equal("integer", SchemaTypeMapper.Map(typeof(int?)));
        }

        [Fact]
        public void ShouldConvertToBoolean_WhenNullable()
        {
            Assert.Equal("boolean", SchemaTypeMapper.Map(typeof(bool?)));
        }

        [Fact]
        public void ShouldConvertToNumber_WhenNullableDecimal()
        {
            Assert.Equal("number", SchemaTypeMapper.Map(typeof(decimal?)));
        }

        [Fact]
        public void ShouldConvertToNumber_WhenNullableFloat()
        {
            Assert.Equal("number", SchemaTypeMapper.Map(typeof(float?)));
        }

        [Fact]
        public void ShouldConvertToString_WhenNullableDateTime()
        {
            Assert.Equal("string", SchemaTypeMapper.Map(typeof(DateTime?)));
        }



        [Fact]
        public void ShouldGetAttributeInteger()
        {
            Assert.Equal("\"integer\"", SchemaTypeMapper.GetAttribute(typeof(int)));
        }

        [Fact]
        public void ShouldGetAttributeString()
        {
            Assert.Equal("\"string\"", SchemaTypeMapper.GetAttribute(typeof(string)));
        }

        [Fact]
        public void ShouldGetAttributeBoolean()
        {
            Assert.Equal("\"boolean\"", SchemaTypeMapper.GetAttribute(typeof(bool)));
        }

        [Fact]
        public void ShouldGetAttributeNumber_WhenDecimal()
        {
            Assert.Equal("\"number\"", SchemaTypeMapper.GetAttribute(typeof(decimal)));
        }

        [Fact]
        public void ShouldGetAttributeNumber_WhenFloat()
        {
            Assert.Equal("\"number\"", SchemaTypeMapper.GetAttribute(typeof(float)));
        }

        [Fact]
        public void ShouldGetAttributeString_WhenDateTime()
        {
            Assert.Equal("\"string\"", SchemaTypeMapper.GetAttribute(typeof(DateTime)));
        }


        [Fact]
        public void ShouldGetAttributeInteger_WhenNullable()
        {
            Assert.Equal("[\"integer\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(int?)));
        }

        [Fact]
        public void ShouldGetAttributeBoolean_WhenNullable()
        {
            Assert.Equal("[\"boolean\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(bool?)));
        }

        [Fact]
        public void ShouldGetAttributeNumber_WhenNullableDecimal()
        {
            Assert.Equal("[\"number\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(decimal?)));
        }

        [Fact]
        public void ShouldGetAttributeNumber_WhenNullableFloat()
        {
            Assert.Equal("[\"number\",\"null\"]", SchemaTypeMapper.GetAttribute(typeof(float?)));
        }

        [Fact]
        public void ShouldGetAttributeString_WhenNullableDateTime()
        {
            Assert.Equal("\"string\"", SchemaTypeMapper.GetAttribute(typeof(DateTime?)));
        }

    }
}