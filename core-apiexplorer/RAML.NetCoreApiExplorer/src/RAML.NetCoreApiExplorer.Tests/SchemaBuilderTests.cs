using FstabExplorerTest.Fstab.Models;
using Newtonsoft.Json.Schema;
using RAML.WebApiExplorer;
using RAML.WebApiExplorer.Tests.Types;
using Xunit;

namespace RAML.NetCoreApiExplorer.Tests
{
	
    public class SchemaBuilderTests
	{
	    private readonly SchemaBuilder schemaBuilder = new SchemaBuilder();

		[Fact]
		public void ShouldParseTypeWithNestedTypes()
		{
			var schema = schemaBuilder.Get(typeof (ForksPostResponse));
			Assert.True(schema.Contains("\"Owner\":"));
			var obj = JsonSchema.Parse(schema);
			Assert.NotNull(obj);
		}

		[Fact]
		public void ShouldParseTypeWithNestedTypeWhereFirstTypeHasNoSettableProperties() {
			var schema = schemaBuilder.Get(typeof(WebLocation));
			Assert.True(schema.Contains("\"Location\":"));
			var obj = JsonSchema.Parse(schema);
			Assert.NotNull(obj);
		}

		[Fact]
		public void ShouldParseArray()
		{
			var schema = schemaBuilder.Get(typeof(Owner[]));
			var obj = JsonSchema.Parse(schema);
			Assert.NotNull(obj);
			Assert.Equal(JsonSchemaType.Array, obj.Type);
		}

		[Fact]
		public void ShouldParseComplexType()
		{
			var schema = schemaBuilder.Get(typeof(SearchGetResponse));
			var obj = JsonSchema.Parse(schema);
			Assert.NotNull(obj);
		}

        [Fact]
        public void ShouldParseTypeWithSubclasses()
        {
            var schema = schemaBuilder.Get(typeof(Entry));
            Assert.True(schema.Contains("\"oneOf\":"));
            Assert.True(schema.Contains("\"definitions\":"));
            var obj = JsonSchema.Parse(schema);
            Assert.NotNull(obj);
        }

        [Fact]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            var schema = schemaBuilder.Get(typeof(Employee));
            Assert.True(schema.Contains("\"$ref\": \"Employee\""));
            var obj = JsonSchema.Parse(schema);
            Assert.NotNull(obj);
        }

        [Fact]
        public void ShouldGenerateAnnotations()
        {
            var schema = schemaBuilder.Get(typeof(AnnotatedObject));
            var obj = JsonSchema.Parse(schema);
            Assert.NotNull(obj);
            Assert.True(schema.Contains("\"Age\": { \"type\": \"integer\", \"minimum\": 18, \"maximum\": 120"));
            Assert.True(schema.Contains("\"Weight\": { \"type\": \"number\", \"minimum\": 20.50, \"maximum\": 300.50"));
            Assert.True(schema.Contains("\"LastName\": { \"type\": \"string\", \"required\": true"));
            Assert.True(schema.Contains("\"City\": { \"type\": \"string\", \"maxLength\": 255}"));
            Assert.True(schema.Contains("\"State\": { \"type\": \"string\", \"minLength\": 2}"));
            Assert.True(schema.Contains("\"Person\":\r\n      { \r\n        \"type\": \"object\",\r\n        \"required\": true,"));
        }
    }
}
