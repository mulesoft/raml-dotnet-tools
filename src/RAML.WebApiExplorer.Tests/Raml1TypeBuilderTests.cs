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
        public void ShouldParseUriAsString()
        {
            raml1TypeBuilder.Add(typeof(WebLocation));
            Assert.AreEqual(1, raml1Types.Count);
            Assert.IsTrue(!raml1Types.ContainsKey("Uri"));
            Assert.IsTrue(raml1Types["WebLocation"].ToString().Contains("properties:"));
            Assert.IsTrue(raml1Types["WebLocation"].ToString().Contains("BaseUri:"));
            Assert.IsTrue(raml1Types["WebLocation"].ToString().Contains("Location:"));
        }

		[Test]
		public void ShouldSkipTypeThatHasNoSettableProperties()
        {
            raml1TypeBuilder.Add(typeof(TypeWithReadOnlyProperties));
            Assert.AreEqual(1, raml1Types.Count);
            Assert.IsFalse(raml1Types["TypeWithReadOnlyProperties"].ToString().Contains("ReadOnlyProp"));
            Assert.IsFalse(raml1Types["TypeWithReadOnlyProperties"].ToString().Contains("ReadOnlyObject"));
		}

		[Test]
		public void ShouldParseArray()
		{
			var raml1Type = raml1TypeBuilder.Add(typeof(Owner[]));
            Assert.AreEqual(1, raml1Types.Count);
			Assert.IsTrue(raml1Types.ContainsKey("Owner"));
            Assert.AreEqual("Owner[]", raml1Type);
		}

        [Test]
        public void ShouldParseDictionary()
        {
            raml1TypeBuilder.Add(typeof(IDictionary<string, Owner>));
            Assert.AreEqual(2, raml1Types.Count);
            Assert.IsTrue(raml1Types.ContainsKey("Owner"));
            Assert.IsTrue(raml1Types.ContainsKey("OwnerMap"));
            Assert.IsTrue(raml1Types["OwnerMap"].ToString().Contains("[]:"));
            Assert.IsTrue(raml1Types["OwnerMap"].ToString().Contains("type: Owner"));
        }

		[Test]
		public void ShouldParseComplexType()
		{
			raml1TypeBuilder.Add(typeof(SearchGetResponse));
            Assert.AreEqual(5, raml1Types.Count);
		}

        [Test, Ignore] // it should not be necesary
        public void ShouldGetSubclassesOfType()
        {
            raml1TypeBuilder.Add(typeof(Entry));
            Assert.AreEqual(6, raml1Types.Count);
            Assert.IsTrue(raml1Types["StoragediskUUID"].ToString().Contains("type: Storage"));
        }

        [Test]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            raml1TypeBuilder.Add(typeof(Employee));
            Assert.AreEqual(2, raml1Types.Count);
            Assert.IsTrue(raml1Types["Employee"].ToString().Contains("type: Person"));
        }

        [Test]
        public void ShouldGenerateAnnotations()
        {
            raml1TypeBuilder.Add(typeof(AnnotatedObject));
            var raml1Type = raml1Types["AnnotatedObject"].ToString();
            Assert.IsTrue(raml1Type.Contains("minimum: 18"));
            Assert.IsTrue(raml1Type.Contains("maximum: 120"));
            Assert.IsTrue(raml1Type.Contains("minimum: 20.50"));
            Assert.IsTrue(raml1Type.Contains("maximum: 300.50"));
            Assert.IsTrue(raml1Type.Contains("maxLength: 255"));
            Assert.IsTrue(raml1Type.Contains("minLength: 2"));
        }
    }
}
