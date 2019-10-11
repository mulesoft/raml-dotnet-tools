using System.IO;
using FstabExplorerTest.Fstab.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RAML.WebApiExplorer.Tests.Types;

namespace RAML.WebApiExplorer.Tests
{
	[TestClass]
    public class SchemaBuilderTests
	{
	    private readonly SchemaBuilder schemaBuilder = new SchemaBuilder();

		[TestMethod]
		public void ShouldParseTypeWithNestedTypes()
		{
			var schema = schemaBuilder.Get(typeof (ForksPostResponse));
			Assert.IsTrue(schema.Contains("\"Owner\":"));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void ShouldParseTypeWithNestedTypeWhereFirstTypeHasNoSettableProperties() {
			var schema = schemaBuilder.Get(typeof(WebLocation));
			Assert.IsTrue(schema.Contains("\"Location\":"));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
		}

		[TestMethod]
		public void ShouldParseArray()
		{
			var schema = schemaBuilder.Get(typeof(Owner[]));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
			Assert.AreEqual(JsonSchemaType.Array, obj.Type);
		}

		[TestMethod]
		public void ShouldParseComplexType()
		{
			var schema = schemaBuilder.Get(typeof(SearchGetResponse));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
		}

        [TestMethod]
        public void ShouldParseTypeWithSubclasses()
        {
            var schema = schemaBuilder.Get(typeof(Entry));
            Assert.IsTrue(schema.Contains("\"oneOf\":"));
            Assert.IsTrue(schema.Contains("\"definitions\":"));
            var obj = JsonSchema.Parse(schema);
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            var schema = schemaBuilder.Get(typeof(Employee));
            Assert.IsTrue(schema.Contains("\"$ref\": \"Employee\""));
            var obj = JsonSchema.Parse(schema);
            Assert.IsNotNull(obj);
        }

        [TestMethod]
        public void ShouldGenerateAnnotations()
        {
            var schema = schemaBuilder.Get(typeof(AnnotatedObject));
            var obj = JsonSchema.Parse(schema);
            Assert.IsNotNull(obj);
            Assert.IsTrue(schema.Contains("\"Age\": { \"type\": \"integer\", \"minimum\": 18, \"maximum\": 120"));
            Assert.IsTrue(schema.Contains("\"Weight\": { \"type\": \"number\", \"minimum\": 20.50, \"maximum\": 300.50"));
            Assert.IsTrue(schema.Contains("\"LastName\": { \"type\": \"string\", \"required\": true"));
            Assert.IsTrue(schema.Contains("\"City\": { \"type\": \"string\", \"maxLength\": 255}"));
            Assert.IsTrue(schema.Contains("\"State\": { \"type\": \"string\", \"minLength\": 2}"));
            Assert.IsTrue(schema.Contains("\"Person\":\r\n      { \r\n        \"type\": \"object\",\r\n        \"required\": true,"));
        }
    }
}
