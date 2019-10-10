using Newtonsoft.Json.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class NetTypeMapperTests
    {
        [TestMethod]
        public void ShouldConvertIntegerToInt()
        {
            Assert.AreEqual("int", NetTypeMapper.GetNetType(JsonSchemaType.Integer, null));
        }

        [TestMethod]
        public void ShouldConvertToString()
        {
            Assert.AreEqual("string", NetTypeMapper.GetNetType(JsonSchemaType.String, null));
        }

        [TestMethod]
        public void ShouldConvertToBool()
        {
            Assert.AreEqual("bool", NetTypeMapper.GetNetType(JsonSchemaType.Boolean, null));
        }

        [TestMethod]
        public void ShouldConvertToDecimal()
        {
            Assert.AreEqual("decimal", NetTypeMapper.GetNetType(JsonSchemaType.Float, null));
        }

        [TestMethod]
        public void ShouldConvertToByteArrayWhenFile()
        {
            Assert.AreEqual("byte[]", NetTypeMapper.GetNetType("file", null));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeWhenDate()
        {
            Assert.AreEqual("DateTime", NetTypeMapper.GetNetType("date", null));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeWhenDatetime()
        {
            Assert.AreEqual("DateTime", NetTypeMapper.GetNetType("datetime", null));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeWhenDateOnly()
        {
            Assert.AreEqual("DateTime", NetTypeMapper.GetNetType("date-only", null));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeWhenTimeOnly()
        {
            Assert.AreEqual("DateTime", NetTypeMapper.GetNetType("time-only", null));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeWhenDatetimeOnly()
        {
            Assert.AreEqual("DateTime", NetTypeMapper.GetNetType("datetime-only", null));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeOffsetWhenRfc2616()
        {
            Assert.AreEqual("DateTimeOffset", NetTypeMapper.GetNetType("datetime", "rfc2616"));
        }

        [TestMethod]
        public void ShouldConvertToDateTimeWhenRfc3339()
        {
            Assert.AreEqual("DateTime", NetTypeMapper.GetNetType("datetime", "rfc3339"));
        }

        [TestMethod]
        public void ShouldConvertToLongWhenFormatIsLong()
        {
            Assert.AreEqual("long", NetTypeMapper.GetNetType("number", "long"));
        }

        [TestMethod]
        public void ShouldConvertToLongWhenFormatIsInt64()
        {
            Assert.AreEqual("long", NetTypeMapper.GetNetType("number", "int64"));
        }

        [TestMethod]
        public void ShouldConvertToIntWhenFormatIsInt32()
        {
            Assert.AreEqual("int", NetTypeMapper.GetNetType("number", "int32"));
        }

        [TestMethod]
        public void ShouldConvertToIntWhenFormatIsInt()
        {
            Assert.AreEqual("int", NetTypeMapper.GetNetType("number", "int"));
        }

        [TestMethod]
        public void ShouldConvertToShortWhenFormatIsInt16()
        {
            Assert.AreEqual("short", NetTypeMapper.GetNetType("number", "int16"));
        }

        [TestMethod]
        public void ShouldConvertToByteWhenFormatIsInt8()
        {
            Assert.AreEqual("byte", NetTypeMapper.GetNetType("number", "int8"));
        }

        [TestMethod]
        public void ShouldConvertToIntWhenInteger()
        {
            Assert.AreEqual("int", NetTypeMapper.GetNetType("integer", null));
        }

        [TestMethod]
        public void ShouldConvertToDecimalWhenNumber()
        {
            Assert.AreEqual("decimal", NetTypeMapper.GetNetType("number", null));
        }

        [TestMethod]
        public void ShouldConvertToFloatWhenFormatIsFloat()
        {
            Assert.AreEqual("float", NetTypeMapper.GetNetType("number", "float"));
        }

        [TestMethod]
        public void ShouldConvertToDoubleWhenFormatIsDouble()
        {
            Assert.AreEqual("double", NetTypeMapper.GetNetType("number", "double"));
        }

        [TestMethod]
        public void ShouldTrimBeforeConvertingString()
        {
            Assert.AreEqual("string", NetTypeMapper.Map(" string "));
        }

        [TestMethod]
        public void ShouldTrimBeforeConvertingInt()
        {
            Assert.AreEqual("int", NetTypeMapper.Map(" integer "));
        }
    }
}