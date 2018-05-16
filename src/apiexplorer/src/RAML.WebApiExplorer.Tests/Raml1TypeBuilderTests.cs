using System.Collections.Generic;
using FstabExplorerTest.Fstab.Models;
using NUnit.Framework;
using AMF.Parser.Model;
using RAML.WebApiExplorer.Tests.Types;
using static RAML.WebApiExplorer.ApiExplorerService;
using System.Linq;

namespace RAML.WebApiExplorer.Tests
{
	[TestFixture]
    public class TypeToShapeConverterTests
	{
        private TypeToShapeConverter typesToShapeConverter;

	    [SetUp]
	    public void TestSetup()
	    {
            typesToShapeConverter = new TypeToShapeConverter();
	    }

	    [Test]
		public void ShouldParseTypeWithNestedTypes()
		{
			var raml1Type = typesToShapeConverter.Convert(typeof (ForksPostResponse));
            Assert.IsNotNull(raml1Type);
            Assert.IsTrue(typesToShapeConverter.Types.ContainsKey("RAML.WebApiExplorer.Tests.Types.ForksPostResponse"));
            Assert.IsTrue(typesToShapeConverter.Types.ContainsKey("RAML.WebApiExplorer.Tests.Types.Owner"));
		}

        [Test]
        public void ShouldParseUriAsString()
        {
            typesToShapeConverter.Convert(typeof(WebLocation));
            Assert.AreEqual(1, typesToShapeConverter.Types.Count);
            Assert.IsTrue(!typesToShapeConverter.Types.ContainsKey("Uri"));
            var webLocation = typesToShapeConverter.Types["RAML.WebApiExplorer.Tests.Types.WebLocation"] as NodeShape;
            Assert.AreEqual(2, webLocation.Properties.Count());
            Assert.IsTrue(webLocation.Properties.Any(p => p.Path == "BaseUri"));
            Assert.IsTrue(webLocation.Properties.Any(p => p.Path == "Location"));
        }

        [Test]
        public void ShouldSkipTypeThatHasNoSettableProperties()
        {
            typesToShapeConverter.Convert(typeof(TypeWithReadOnlyProperties));
            Assert.AreEqual(1, typesToShapeConverter.Types.Count);
            var readOnlyType = typesToShapeConverter.Types["RAML.WebApiExplorer.Tests.Types.TypeWithReadOnlyProperties"] as NodeShape;
            Assert.IsFalse(readOnlyType.Properties.Any(p => p.Path == "ReadOnlyProp"));
            Assert.IsFalse(readOnlyType.Properties.Any(p => p.Path == "ReadOnlyObject"));
		}

		[Test]
		public void ShouldParseArray()
		{
			var raml1Type = typesToShapeConverter.Convert(typeof(Owner[]));
            Assert.AreEqual(2, typesToShapeConverter.Types.Count);
			Assert.IsTrue(typesToShapeConverter.Types.ContainsKey("RAML.WebApiExplorer.Tests.Types.Owner"));
            Assert.AreEqual("RAML.WebApiExplorer.Tests.Types.Owner", ((ArrayShape)typesToShapeConverter.Types["RAML.WebApiExplorer.Tests.Types.Owner[]"]).Items.Name);
		}

        [Test]
        public void ShouldParseArrayOfPrimitives()
        {
            var type = typesToShapeConverter.Convert(typeof(int[]));
            Assert.AreEqual(0, typesToShapeConverter.Types.Count);
            Assert.AreEqual("integer[]", type);
        }

        [Test]
        public void ShouldParseListOfPrimitives()
        {
            var type = typesToShapeConverter.Convert(typeof(List<int>));
            Assert.AreEqual(0, typesToShapeConverter.Types.Count);
            Assert.AreEqual("integer[]", type);
        }

        [Test]
        public void ShouldParseArrayOfArray()
        {
            var type = typesToShapeConverter.Convert(typeof(List<int[]>));
            Assert.AreEqual(0, typesToShapeConverter.Types.Count);
            Assert.AreEqual("integer[][]", type);
        }

        //[Test]
        //public void ShouldParseDictionary()
        //{
        //    typesToShapeConverter.Convert(typeof(IDictionary<string, Owner>));
        //    Assert.AreEqual(2, typesToShapeConverter.Types.Count);
        //    Assert.IsTrue(typesToShapeConverter.Types.ContainsKey("Owner"));
        //    Assert.IsTrue(typesToShapeConverter.Types.ContainsKey("OwnerMap"));
        //    var node = typesToShapeConverter.Types["OwnerMap"] as NodeShape;
        //    Assert.IsTrue(node.Properties.Any(p => p.Path == "[]"));
        //    Assert.AreEqual("Owner", node.Properties.First(p => p.Path == "[]"].Type);
        //}

		[Test]
		public void ShouldParseComplexType()
		{
			typesToShapeConverter.Convert(typeof(SearchGetResponse));
            Assert.AreEqual(6, typesToShapeConverter.Types.Count);
		}

        [Test] // it should not be necesary
        public void ShouldGetSubclassesOfType()
        {
            typesToShapeConverter.Convert(typeof(Entry));
            Assert.AreEqual(6, typesToShapeConverter.Types.Count);
            Assert.AreEqual("Storage", typesToShapeConverter.Types["StoragediskUUID"].LinkTargetName);
        }

        [Test]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            typesToShapeConverter.Convert(typeof(Employee));
            Assert.AreEqual(2, typesToShapeConverter.Types.Count);
            Assert.AreEqual("RAML.WebApiExplorer.Tests.Types.Person", typesToShapeConverter.Types["RAML.WebApiExplorer.Tests.Types.Employee"].LinkTargetName);
        }

        [Test]
        public void ShouldGenerateAnnotations()
        {
            typesToShapeConverter.Convert(typeof(AnnotatedObject));
            var raml1Type = typesToShapeConverter.Types["RAML.WebApiExplorer.Tests.Types.AnnotatedObject"] as NodeShape;
            var age = raml1Type.Properties.First(p => p.Path == "Age").Range as ScalarShape;
            Assert.AreEqual("18", age.Minimum);
            Assert.AreEqual("120", age.Maximum);
            var weigth = raml1Type.Properties.First(p => p.Path == "Weight").Range as ScalarShape;
            Assert.AreEqual(20.5.ToString(), weigth.Minimum);
            Assert.AreEqual(300.5.ToString(), weigth.Maximum);
            Assert.AreEqual(255, ((ScalarShape)raml1Type.Properties.First(p => p.Path == "City").Range).MaxLength);
            Assert.AreEqual(2, ((ScalarShape)raml1Type.Properties.First(p => p.Path == "State").Range).MinLength);
        }
    }
}
