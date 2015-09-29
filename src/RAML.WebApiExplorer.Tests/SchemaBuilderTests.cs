using System.IO;
using FstabExplorerTest.Fstab.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using RAML.WebApiExplorer.Tests.Types;

namespace RAML.WebApiExplorer.Tests
{
	[TestFixture]
    public class SchemaBuilderTests
	{
	    private readonly SchemaBuilder schemaBuilder = new SchemaBuilder();

		[Test]
		public void ShouldParseTypeWithNestedTypes()
		{
			var schema = schemaBuilder.Get(typeof (ForksPostResponse));
			Assert.IsTrue(schema.Contains("\"Owner\":"));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
		}

		[Test]
		public void ShouldParseTypeWithNestedTypeWhereFirstTypeHasNoSettableProperties() {
			var schema = schemaBuilder.Get(typeof(WebLocation));
			Assert.IsTrue(schema.Contains("\"Location\":"));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
		}

		[Test]
		public void ShouldParseArray()
		{
			var schema = schemaBuilder.Get(typeof(Owner[]));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
			Assert.AreEqual(JsonSchemaType.Array, obj.Type);
		}

		[Test]
		public void ShouldParseComplexType()
		{
			var schema = schemaBuilder.Get(typeof(SearchGetResponse));
			var obj = JsonSchema.Parse(schema);
			Assert.IsNotNull(obj);
		}

        [Test]
        public void ShouldParseTypeWithSubclasses()
        {
            var schema = schemaBuilder.Get(typeof(Entry));
            Assert.IsTrue(schema.Contains("\"oneOf\":"));
            Assert.IsTrue(schema.Contains("\"definitions\":"));
            var obj = JsonSchema.Parse(schema);
            Assert.IsNotNull(obj);
        }

        [Test]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            var schema = schemaBuilder.Get(typeof(Employee));
            Assert.IsTrue(schema.Contains("\"$ref\": \"Employee\""));
            var obj = JsonSchema.Parse(schema);
            Assert.IsNotNull(obj);
        }

    }
}
