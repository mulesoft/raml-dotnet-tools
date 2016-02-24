using System.Collections.Generic;
using System.IO;
using FstabExplorerTest.Fstab.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Schema;
using NUnit.Framework;
using RAML.WebApiExplorer.Tests.Types;

namespace RAML.WebApiExplorer.Tests
{
	[TestFixture]
    public class Raml1TypeBuilderTests
	{
        private readonly Raml1TypeBuilder raml1TypeBuilder = new Raml1TypeBuilder(raml1Types);
	    private static readonly MyOrderedDictionary raml1Types = new MyOrderedDictionary();

	    [SetUp]
	    public void TestSetup()
	    {
            raml1Types.Clear();
	    }

	    [Test]
		public void ShouldParseTypeWithNestedTypes()
		{
			var raml1Type = raml1TypeBuilder.Add(typeof (ForksPostResponse));
            Assert.IsNotNullOrEmpty(raml1Type);
            Assert.IsTrue(raml1Types.ContainsKey("ForksPostResponse"));
            Assert.IsTrue(raml1Types.ContainsKey("Owner"));
		}

		[Test]
		public void ShouldParseTypeWithNestedTypeWhereFirstTypeHasNoSettableProperties()
        {
			var raml1Type = raml1TypeBuilder.Add(typeof(WebLocation));
            Assert.IsTrue(raml1Types.ContainsKey("Uri"));
			Assert.IsNotNullOrEmpty(raml1Type);
		}

		[Test]
		public void ShouldParseArray()
		{
			var raml1Type = raml1TypeBuilder.Add(typeof(Owner[]));
            Assert.AreEqual(1, raml1Types.Count);
			Assert.IsTrue(raml1Types.ContainsKey("Owner"));
            Assert.AreEqual("type: Owner[]", raml1Type);
		}

        [Test]
        public void ShouldParseDictionary()
        {
            var raml1Type = raml1TypeBuilder.Add(typeof(IDictionary<string, Owner>));
            Assert.AreEqual(2, raml1Types.Count);
            Assert.IsTrue(raml1Types.ContainsKey("Owner"));
            Assert.IsTrue(raml1Types.ContainsKey("OwnerMap"));
            Assert.IsTrue(raml1Type.Contains("[]:"));
            Assert.IsTrue(raml1Type.Contains("type: Owner"));
        }

		[Test]
		public void ShouldParseComplexType()
		{
			var raml1Type = raml1TypeBuilder.Add(typeof(SearchGetResponse));
		}

        [Test]
        public void ShouldParseTypeWithSubclasses()
        {
            var raml1Type = raml1TypeBuilder.Add(typeof(Entry));
        }

        [Test]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            var raml1Type = raml1TypeBuilder.Add(typeof(Employee));
        }

        [Test]
        public void ShouldGenerateAnnotations()
        {
            var raml1Type = raml1TypeBuilder.Add(typeof(AnnotatedObject));
            //var obj = JsonSchema.Parse(schema);
            //Assert.IsNotNull(obj);
            //Assert.IsTrue(schema.Contains("\"Age\": { \"type\": \"integer\", \"minimum\": 18, \"maximum\": 120"));
            //Assert.IsTrue(schema.Contains("\"Weight\": { \"type\": \"number\", \"minimum\": 20.50, \"maximum\": 300.50"));
            //Assert.IsTrue(schema.Contains("\"LastName\": { \"type\": \"string\", \"required\": true"));
            //Assert.IsTrue(schema.Contains("\"City\": { \"type\": \"string\", \"maxLength\": 255}"));
            //Assert.IsTrue(schema.Contains("\"State\": { \"type\": \"string\", \"minLength\": 2}"));
            //Assert.IsTrue(schema.Contains("\"Person\":\r\n      { \r\n        \"type\": \"object\",\r\n        \"required\": true,"));
        }
    }
}
