using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using Raml.Tools.XML;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class XmlSchemaParserTests
    {
        private static XmlSchemaParser parser;

        [TestInitialize]
        public void SetUp()
        {
            parser = new XmlSchemaParser();
        }

        [TestMethod]
        public void should_not_add_object_to_collection_when_no_properties()
        {
            const string emptySchema = "<xsd:schema targetNamespace=\"http://www.example.com/IPO\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\" xmlns:ipo=\"http://www.example.com/IPO\"></xsd:schema>";
            var apiObjects = new Dictionary<string, ApiObject>();
            var obj = parser.Parse("key", emptySchema, apiObjects, "Generated");
            Assert.AreEqual(null, obj);
            Assert.AreEqual(0, apiObjects.Count);
        }

        [TestMethod]
        public void should_avoid_types_duplication()
        {
            var schema = File.ReadAllText(@"files\ipo.xsd");
            var apiObjects = new Dictionary<string, ApiObject>
            {
                { "PurchaseOrderType", new ApiObject() }
            };
            var obj = parser.Parse("key", schema, apiObjects, "Generated");
            Assert.AreEqual(null, obj);
            Assert.AreEqual(1, apiObjects.Count);
        }

        [TestMethod]
        public void should_parse_all_objects_in_schema_ipo()
        {
            var schema = File.ReadAllText(@"files\ipo.xsd");
            var objects = new Dictionary<string, ApiObject>();
            var obj = parser.Parse("key", schema, objects, "Generated");
            Assert.IsFalse(string.IsNullOrWhiteSpace(obj.GeneratedCode));
            Assert.AreEqual(11, objects.Count);
        }

        [TestMethod]
        public void should_parse_all_objects_in_schema_75039()
        {
            var schema = File.ReadAllText(@"files\75039.xsd");
            var objects = new Dictionary<string, ApiObject>();
            parser.Parse("key", schema, objects, "Generated");

            Assert.AreEqual(3, objects.Count);
        }

        [TestMethod]
        public void should_parse_0_objects_when_only_annotations()
        {
            var schema = File.ReadAllText(@"files\annotations00101m1.xsd");
            var objects = new Dictionary<string, ApiObject>();
            var obj = parser.Parse("key", schema, objects, "Generated");
            Assert.AreEqual(null, obj);
            Assert.AreEqual(0, objects.Count);
        }

        [TestMethod]
        public void should_parse_all_objects_in_schema_test67200()
        {
            var schema = File.ReadAllText(@"files\test67200.xsd");
            var objects = new Dictionary<string, ApiObject>();
            parser.Parse("key", schema, objects, "Generated");

            Assert.AreEqual(2, objects.Count);
        }
    }
}
