using System.Collections.Generic;
using FstabExplorerTest.Fstab.Models;
using Raml.Parser.Expressions;
using AMF.NetCoreApiExplorer.Tests.Types;
using AMF.WebApiExplorer;
using AMF.WebApiExplorer.Tests.Types;
using Xunit;

namespace AMF.NetCoreApiExplorer.Tests
{
    public class Raml1TypeBuilderTests
	{
        private readonly Raml1TypeBuilder raml1TypeBuilder;
	    private readonly RamlTypesOrderedDictionary raml1Types;

	    public Raml1TypeBuilderTests()
	    {
            raml1Types = new RamlTypesOrderedDictionary();
            raml1TypeBuilder = new Raml1TypeBuilder(raml1Types);
	    }

	    [Fact]
		public void ShouldParseTypeWithNestedTypes()
		{
			var raml1Type = raml1TypeBuilder.Add(typeof (ForksPostResponse));
            Assert.NotNull(raml1Type);
            Assert.True(raml1Types.ContainsKey("ForksPostResponse"));
            Assert.True(raml1Types.ContainsKey("Owner"));
		}

        [Fact]
        public void ShouldParseUriAsString()
        {
            raml1TypeBuilder.Add(typeof(WebLocation));
            Assert.Equal(1, raml1Types.Count);
            Assert.True(!raml1Types.ContainsKey("Uri"));
            Assert.Equal(2, raml1Types.GetByKey("WebLocation").Object.Properties.Count);
            Assert.True(raml1Types.GetByKey("WebLocation").Object.Properties.ContainsKey("BaseUri"));
            Assert.True(raml1Types.GetByKey("WebLocation").Object.Properties.ContainsKey("Location"));
        }

		[Fact]
		public void ShouldSkipTypeThatHasNoSettableProperties()
        {
            raml1TypeBuilder.Add(typeof(TypeWithReadOnlyProperties));
            Assert.Equal(1, raml1Types.Count);
            Assert.False(raml1Types.GetByKey("TypeWithReadOnlyProperties").Object.Properties.ContainsKey("ReadOnlyProp"));
            Assert.False(raml1Types.GetByKey("TypeWithReadOnlyProperties").Object.Properties.ContainsKey("ReadOnlyObject"));
		}

		[Fact]
		public void ShouldParseArray()
		{
			raml1TypeBuilder.Add(typeof(Owner[]));
            Assert.Equal(2, raml1Types.Count);
			Assert.True(raml1Types.ContainsKey("Owner"));
            Assert.Equal("Owner", raml1Types.GetByKey("ListOfOwner").Array.Items.Type);
		}

        [Fact]
        public void ShouldParseArrayOfPrimitives()
        {
            var type = raml1TypeBuilder.Add(typeof(int[]));
            Assert.Equal(0, raml1Types.Count);
            Assert.Equal("integer[]", type);
        }

        [Fact]
        public void ShouldParseListOfPrimitives()
        {
            var type = raml1TypeBuilder.Add(typeof(List<int>));
            Assert.Equal(0, raml1Types.Count);
            Assert.Equal("integer[]", type);
        }

        [Fact]
        public void ShouldParseArrayOfArray()
        {
            var type = raml1TypeBuilder.Add(typeof(List<int[]>));
            Assert.Equal(0, raml1Types.Count);
            Assert.Equal("integer[][]", type);
        }

        [Fact]
        public void ShouldParseDictionary()
        {
            raml1TypeBuilder.Add(typeof(IDictionary<string, Owner>));
            Assert.Equal(2, raml1Types.Count);
            Assert.True(raml1Types.ContainsKey("Owner"));
            Assert.True(raml1Types.ContainsKey("OwnerMap"));
            Assert.True(raml1Types.GetByKey("OwnerMap").Object.Properties.ContainsKey("[]"));
            Assert.Equal("Owner", raml1Types.GetByKey("OwnerMap").Object.Properties["[]"].Type);
        }

		[Fact]
		public void ShouldParseComplexType()
		{
			raml1TypeBuilder.Add(typeof(SearchGetResponse));
            Assert.Equal(6, raml1Types.Count);
		}

        //[Fact]
        //public void ShouldGetSubclassesOfType()
        //{
        //    raml1TypeBuilder.Add(typeof(Entry));
        //    Assert.Equal(6, raml1Types.Count);
        //    Assert.Equal("Storage", raml1Types.GetByKey("StoragediskUUID").Type);
        //}

        [Fact]
        public void ShouldParseTypeWithRecursiveTypes()
        {
            raml1TypeBuilder.Add(typeof(Employee));
            Assert.Equal(2, raml1Types.Count);
            Assert.Equal("Person", raml1Types.GetByKey("Employee").Type);
        }

        [Fact]
        public void ShouldGenerateAnnotations()
        {
            raml1TypeBuilder.Add(typeof(AnnotatedObject));
            var raml1Type = raml1Types.GetByKey("AnnotatedObject");
            Assert.Equal((decimal?)18.00, raml1Type.Object.Properties["Age"].Scalar.Minimum);
            Assert.Equal((decimal?)120.00, raml1Type.Object.Properties["Age"].Scalar.Maximum);
            Assert.Equal((decimal?)20.50, raml1Type.Object.Properties["Weight"].Scalar.Minimum);
            Assert.Equal((decimal?)300.50, raml1Type.Object.Properties["Weight"].Scalar.Maximum);
            Assert.Equal(255, raml1Type.Object.Properties["City"].Scalar.MaxLength);
            Assert.Equal(2, raml1Type.Object.Properties["State"].Scalar.MinLength);
        }
    }
}
