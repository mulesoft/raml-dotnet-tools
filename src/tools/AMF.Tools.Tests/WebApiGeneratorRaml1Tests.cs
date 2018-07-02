using NUnit.Framework;
using AMF.Parser;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.WebApiGenerator;
using AMF.Tools.Core;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class WebApiGeneratorRaml1Tests
    {

        [Test]
        public async Task ShouldBuildArrays()
        {
            var model = await BuildModel("files/raml1/arrayTypes.raml");
            Assert.AreEqual(5, model.Objects.Count());
        }

        [Test]
        public async Task ShouldBuild_WhenCustomScalar()
        {
            var model = await GetCustomScalarModel();
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldMapAttributes_WhenCustomScalarInObject()
        {
            var model = await BuildModel("files/raml1/customscalar-in-object.raml");
            Assert.AreEqual(3, model.Objects.Count());
            Assert.AreEqual(1, model.Objects.First(o => o.Name == "CustomInt").Properties.Count);
            Assert.AreEqual(1, model.Objects.First(o => o.Name == "CustomString").Properties.Count);
            Assert.AreEqual(0, model.Objects.First(o => o.Name == "CustomInt").Properties.First().Minimum);
            Assert.AreEqual(100, model.Objects.First(o => o.Name == "CustomInt").Properties.First().Maximum);
            Assert.AreEqual(5, model.Objects.First(o => o.Name == "CustomString").Properties.First().MinLength);
            Assert.AreEqual(255, model.Objects.First(o => o.Name == "CustomString").Properties.First().MaxLength);
        }

        [Test]
        public async Task ShouldBuild_WhenMovieType()
        {
            var model = await GetMovieTypeModel();
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldBuildTypes_WhenMovies()
        {
            var model = await GetMoviesModel();
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Movie"));
            Assert.AreEqual(9, model.Objects.First(o => o.Name == "Movie").Properties.Count);
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldDetectArrayTypes_WhenMovies()
        {
            var model = await GetMoviesModel();
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Movie"), model.Controllers.First(o => o.Name == "Movies").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Movie"), model.Controllers.First(o => o.Name == "Movies").Methods.First(m => m.Name == "GetAvailable").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Movie"), model.Controllers.First(o => o.Name == "Search").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Movie"), model.Controllers.First(o => o.Name == "Movies").Methods.First(m => m.Name == "Post").Parameter.Type);
        }

        [Test]
        public async Task ShouldBuild_WhenParameters()
        {
            var model = await GetParametersModel();
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldBuild_WhenTypeExpressions()
        {
            var model = await GetTypeExpressionsModel();
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldBuild_EvenWithDisorderedTypes()
        {
            var model = await BuildModel("files/raml1/typesordering.raml");
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("InvoiceLine"), model.Objects.First(c => c.Name == "Invoice").Properties.First(p => p.Name == "Lines").Type);

            Assert.AreEqual("Artist", model.Controllers.First(c => c.Name == "Artists").Methods.First(m => m.Name == "GetById").ReturnType);
            Assert.AreEqual("Album", model.Controllers.First(c => c.Name == "Albums").Methods.First(m => m.Name == "GetById").ReturnType);
            Assert.AreEqual("Track", model.Controllers.First(c => c.Name == "Tracks").Methods.First(m => m.Name == "GetById").ReturnType);
            Assert.AreEqual("Customer", model.Controllers.First(c => c.Name == "Customers").Methods.First(m => m.Name == "GetById").ReturnType);

            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Artist"), model.Controllers.First(c => c.Name == "Artists").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Album"), model.Controllers.First(c => c.Name == "Albums").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Customer"), model.Controllers.First(c => c.Name == "Customers").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Track"), model.Controllers.First(c => c.Name == "Tracks").Methods.First(m => m.Name == "Get").ReturnType);

            Assert.AreEqual("Artist", model.Controllers.First(c => c.Name == "Artists").Methods.First(m => m.Name == "Post").Parameter.Type);
            Assert.AreEqual("Album", model.Controllers.First(c => c.Name == "Albums").Methods.First(m => m.Name == "Post").Parameter.Type);
            Assert.AreEqual("Customer", model.Controllers.First(c => c.Name == "Customers").Methods.First(m => m.Name == "Post").Parameter.Type);
            Assert.AreEqual("Track", model.Controllers.First(c => c.Name == "Tracks").Methods.First(m => m.Name == "Post").Parameter.Type);

        }


        [Test]
        public async Task ShouldBuild_WhenChinook()
        {
            var model = await BuildModel("files/raml1/chinook-v1.raml");
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("InvoiceLine"), model.Objects.First(c => c.Name == "Invoice").Properties.First(p => p.Name == "Lines").Type);

            Assert.AreEqual("Artist", model.Controllers.First(c => c.Name == "Artists").Methods.First(m => m.Name == "GetById").ReturnType);
            Assert.AreEqual("Album", model.Controllers.First(c => c.Name == "Albums").Methods.First(m => m.Name == "GetById").ReturnType);
            Assert.AreEqual("Track", model.Controllers.First(c => c.Name == "Tracks").Methods.First(m => m.Name == "GetById").ReturnType);
            Assert.AreEqual("Customer", model.Controllers.First(c => c.Name == "Customers").Methods.First(m => m.Name == "GetById").ReturnType);

            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Artist"), model.Controllers.First(c => c.Name == "Artists").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Album"), model.Controllers.First(c => c.Name == "Albums").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Customer"), model.Controllers.First(c => c.Name == "Customers").Methods.First(m => m.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Track"), model.Controllers.First(c => c.Name == "Tracks").Methods.First(m => m.Name == "Get").ReturnType);

            Assert.AreEqual("Artist", model.Controllers.First(c => c.Name == "Artists").Methods.First(m => m.Name == "Post").Parameter.Type);
            Assert.AreEqual("Album", model.Controllers.First(c => c.Name == "Albums").Methods.First(m => m.Name == "Post").Parameter.Type);
            Assert.AreEqual("Customer", model.Controllers.First(c => c.Name == "Customers").Methods.First(m => m.Name == "Post").Parameter.Type);
            Assert.AreEqual("Track", model.Controllers.First(c => c.Name == "Tracks").Methods.First(m => m.Name == "Post").Parameter.Type);
        }

        [Test]
        public async Task ShouldHandleUnionTypes()
        {
            var model = await BuildModel("files/raml1/uniontypes.raml");

            Assert.AreEqual(5, model.Objects.Count());

            Assert.AreEqual(2, model.Objects.First(c => c.Name == "Customer").Properties.Count);
            Assert.AreEqual("Person", model.Objects.First(c => c.Name == "Customer").Properties.First(c => c.Name == "Person").Type);
            Assert.AreEqual("Company", model.Objects.First(c => c.Name == "Customer").Properties.First(c => c.Name == "Company").Type);

            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Person"), model.Objects.First(c => c.Name == "Customers").Properties.First(c => c.Name == "Person").Type);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Company"), model.Objects.First(c => c.Name == "Customers").Properties.First(c => c.Name == "Company").Type);

            Assert.AreEqual(false, model.Objects.First(c => c.Name == "Group").IsArray);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Person"), model.Objects.First(c => c.Name == "Group").Properties.First(c => c.Name == "Person").Type);
            Assert.AreEqual("Company", model.Objects.First(c => c.Name == "Group").Properties.First(c => c.Name == "Company").Type);
        }

        [Test]
        public async Task ShouldHandleXml()
        {
            var model = await BuildModel("files/raml1/ordersXml-v1.raml");

            Assert.AreEqual("PurchaseOrderType", model.Controllers.First().Methods.First(m => m.Verb == "Get").ReturnType);
            Assert.AreEqual("PurchaseOrderType", model.Controllers.First().Methods.First(m => m.Verb == "Post").Parameter.Type);

            Assert.AreEqual(11, model.Objects.Count());
        }

        [Test]
        public async Task ShouldHandleCasing()
        {
            var model = await BuildModel("files/raml1/case.raml");

            Assert.IsNotNull(model.Objects.First(c => c.Name == "Person"));
            //Assert.IsNotNull(model.Objects.First(c => c.Name == "Customer"));
            //Assert.AreEqual("Person", model.Objects.First(c => c.Name == "Customer").BaseClass);

            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Person"), model.Controllers.First(c => c.Name == "Persons").Methods.First(m => m.Verb == "Post").Parameter.Type);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Person"), model.Controllers.First(c => c.Name == "Persons").Methods.First(m => m.Verb == "Get").ReturnType);
        }

        [Test]
        public async Task ShouldDiffientiateBetweenTypesAndBaseTypes()
        {
            var model = await BuildModel("files/raml1/underscore.raml");
            Assert.AreEqual("Links", model.Objects.First(o => o.Name == "Example").Properties.First(c => c.Name == "Links").Type);
            Assert.AreEqual("Link", model.Objects.First(o => o.Name == "Links").Properties.First(c => c.Name == "Self").Type);
            //Assert.AreEqual(3, model.Objects.Count());
        }

        [Test]
        public async Task ShouldBuildDependentTypes()
        {
            var model = await BuildModel("files/raml1/dependentTypes.raml");
            Assert.AreEqual(2, model.Objects.Count());
            Assert.AreEqual("TypeA", model.Controllers.First().Methods.First().Parameter.Type);
        }

        [Test]
        public async Task ShouldHandleComplexQueryParams()
        {
            var model = await BuildModel("files/raml1/queryParams.raml");
            //Assert.AreEqual(1, model.Objects.Count());
            //Assert.AreEqual(true, model.Objects.First().IsScalar);
            Assert.AreEqual("string", model.Controllers.First().Methods.First().QueryParameters.First().Type);
        }

        [Test]
        public async Task ShouldHandleDates()
        {
            var model = await BuildModel("files/raml1/dates.raml");
            Assert.AreEqual(3, model.Objects.Count());
            Assert.AreEqual("DateTime", model.Objects.First(x => x.Name == "Person").Properties.First(x => x.Name == "Born").Type);
            Assert.AreEqual("DateTime", model.Objects.First(x => x.Name == "User").Properties.First(x => x.Name == "Lastaccess").Type);
            Assert.AreEqual("DateTime", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Prop1").Type);
            Assert.AreEqual("DateTime", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Prop2").Type);
            Assert.AreEqual("DateTimeOffset", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Prop3").Type);

            Assert.AreEqual("DateTime", model.Controllers.First(x => x.Name == "Access").Methods.First(x => x.Name == "Post").Parameter.Type);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("DateTime"), model.Controllers.First(x => x.Name == "Access").Methods.First(x => x.Name == "Get").ReturnType);
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("DateTime"), model.Controllers.First(x => x.Name == "Persons").Methods.First(x => x.Name == "Put").Parameter.Type);
            
        }

        [Test]
        public async Task ShouldHandleNumberFormats()
        {
            var model = await BuildModel("files/raml1/numbers.raml");
            Assert.AreEqual("int", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Intprop").Type);
            Assert.AreEqual("int", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Integerprop").Type);
            Assert.AreEqual("int", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Int32prop").Type);
            Assert.AreEqual("long", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Longprop").Type);
            Assert.AreEqual("long", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Int64prop").Type);
            Assert.AreEqual("short", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Int16prop").Type);
            Assert.AreEqual("byte", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Int8prop").Type);
            Assert.AreEqual("float", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Floatprop").Type);
            Assert.AreEqual("double", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Doubleprop").Type);
            Assert.AreEqual("decimal", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Numberprop").Type);
        }

        [Test]
        public async Task ShouldHandleNumberFormatsOnRaml08_v3Schema()
        {
            var model = await BuildModel("files/numbers.raml");
            Assert.AreEqual("int", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Intprop").Type);
            Assert.AreEqual("decimal", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Numberprop").Type);
            Assert.AreEqual("long?", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Longprop").Type);
            Assert.AreEqual("short?", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Shortprop").Type);
        }

        [Test]
        public async Task ShouldHandleNumberFormatsOnRaml08_v4Schema()
        {
            var model = await BuildModel("files/numbers-v4.raml");
            Assert.AreEqual("int", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Intprop").Type);
            Assert.AreEqual("decimal", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Numberprop").Type);
            Assert.AreEqual("long?", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Longprop").Type);
            Assert.AreEqual("short?", model.Objects.First(x => x.Name == "Sample").Properties.First(x => x.Name == "Shortprop").Type);
        }

        [Test]
        public async Task ShouldHandle_SalesOrdersCase()
        {
            var model = await BuildModel("files/raml1/salesOrders.raml");
            Assert.AreEqual(18, model.Objects.Count());
        }

        [Test]
        public async Task ShouldHandle_FileTypes()
        {
            var model = await BuildModel("files/raml1/file-type.raml");
            Assert.AreEqual(1, model.Objects.Count());
        }

        [Test]
        public async Task ShouldHandle_TraitsAtMethodLevel()
        {
            var model = await BuildModel("files/raml1/method-level-traits.raml");
            Assert.IsTrue(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "get").ParametersString.Contains("pagesize"));
            Assert.IsTrue(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "get").ParametersString.Contains("offset"));
            Assert.IsFalse(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "post").ParametersString.Contains("pagesize"));
            Assert.IsFalse(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "post").ParametersString.Contains("offset"));
        }

        [Test]
        public async Task ShouldHandle_TraitsAtResourceLevel()
        {
            var model = await BuildModel("files/raml1/resource-level-traits.raml");
            Assert.IsTrue(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "get").ParametersString.Contains("pagesize"));
            Assert.IsTrue(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "get").ParametersString.Contains("offset"));
            Assert.IsTrue(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "post").ParametersString.Contains("pagesize"));
            Assert.IsTrue(model.Controllers.First().Methods.First(m => m.Verb.ToLower() == "post").ParametersString.Contains("offset"));
        }

        [Test]
        public async Task ShouldParse_RequiredScalarInProperty()
        {
            var model = await BuildModel("files/raml1/movietype.raml");
            Assert.AreEqual(true, model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Name").Required);

        }

        [Test]
        public async Task ShouldParse_OptionalInProperty()
        {
            var model = await BuildModel("files/raml1/movietype.raml");
            Assert.AreEqual(false, model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Rented").Required);
            Assert.AreEqual(false, model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Duration").Required);
            Assert.AreEqual(true, model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Language").Required);
            Assert.AreEqual(false, model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Storyline").Required);
            Assert.AreEqual("decimal?", model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Duration").Type);
            Assert.AreEqual("bool?", model.Objects.First(o => o.Name == "Movie").Properties.First(p => p.Name == "Rented").Type);
        }

        [Test]
        public async Task ShouldApplyParametersOfResourceType()
        {
            var model = await BuildModel("files/raml1/resource-types.raml");
            Assert.AreEqual(1, model.Controllers.First(c => c.Name == "Users").Methods.First(m => m.Verb == "Get").UriParameters.Count());
            Assert.AreEqual(1, model.Controllers.First(c => c.Name == "Users").Methods.First(m => m.Verb == "Post").UriParameters.Count());
            Assert.AreEqual(1, model.Controllers.First(c => c.Name == "Users").Methods.First(m => m.Verb == "Put").UriParameters.Count());
            Assert.AreEqual(3, model.Controllers.First(c => c.Name == "Users").Methods.First(m => m.Verb == "Get").QueryParameters.Count);
        }

        [Test]
        public async Task StringArrayTest()
        {
            var model = await BuildModel("files/raml1/string-array.raml");
            Assert.AreEqual(2, model.Objects.Count());
            Assert.AreEqual("Messages", model.Objects.First(o => o.Name == "Other").Properties.First().Name);
            Assert.AreEqual("Messages", model.Objects.First(o => o.Name == "Some").Properties.First().Name);
            Assert.AreEqual("Some", model.Controllers.First(c => c.Name == "Messages").Methods.First(m => m.Verb == "Post").Parameter.Type);
            Assert.AreEqual("Other", model.Controllers.First(c => c.Name == "Messages").Methods.First(m => m.Verb == "Get").ReturnType);
        }

        [Test]
        public async Task ShouldHandleSimilarSchemas()
        {
            var model = await BuildModel("files/raml1/similar-schemas-ignored.raml");
            Assert.AreEqual(2, model.Objects.Count());
            Assert.AreEqual("Thing", model.Controllers.First(o => o.Name == "Things").Methods.First().ReturnType);
            Assert.AreEqual("Thingy", model.Controllers.First(o => o.Name == "Thingys").Methods.First().ReturnType);
        }

        [Test]
        public async Task ShouldHandleRouteNameContainedInUriParam()
        {
            var model = await BuildModel("files/raml1/applicationId.raml");
            Assert.AreEqual("{applicationId}", model.Controllers.First().Methods.First(m => m.UriParameters.Any()).Url);
            Assert.AreEqual("{applicationId}", model.Controllers.First().Methods.Last(m => m.UriParameters.Any()).Url);
        }

        [Test]
        public async Task ShouldHandleTraitsInLibraries()
        {
            var model = await BuildModel("files/raml1/lib-traits.raml");
            Assert.AreEqual(2, model.Controllers.First().Methods.First().QueryParameters.Count);
            Assert.AreEqual("Year", model.Controllers.First().Methods.First().QueryParameters.First().Name);
        }

        [Test]
        public async Task ShouldHandleNullDescription()
        {
            var model = await BuildModel("files/service-test.raml");
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [Test]
        public async Task ShouldHandleAnyType()
        {
            var model = await BuildModel("files/raml1/anytype.raml");
            Assert.AreEqual(2, model.Controllers.Count());
            Assert.AreEqual(2, model.Objects.Count());
            Assert.AreEqual("object", model.Objects.First(o => o.Name == "Foo").Properties.First(p => p.Name == "Anobj").Type);
            Assert.AreEqual("object", model.Objects.First(o => o.Name != "Foo").Properties.First(p => p.Name == "Baz").Type);
        }

        [Test]
        public async Task ShouldHandleEnumsAtRootLevel()
        {
            var model = await BuildModel("files/raml1/enums-root.raml");
            Assert.AreEqual(2, model.Enums.Count());
            Assert.AreEqual("MyValue1", model.Enums.First(e => e.Name == "MyEnum").Values.First().Name);
            Assert.AreEqual("MyValue3", model.Enums.First(e => e.Name == "MyEnum").Values.Last().Name);
            Assert.AreEqual("MyEnum", model.Objects.First(e => e.Name == "MyUserClass").Properties.First(p => p.Name == "Myenum").Type);
            Assert.AreEqual("Myotherenum", model.Objects.First(e => e.Name == "MyUserClass").Properties.First(p => p.Name == "Myotherenum").Type);
        }

        [Test]
        public async Task ShouldHandleSameNameEnclosingType()
        {
            var model = await BuildModel("files/raml1/enclosing-type-name.raml");
            Assert.IsTrue(model.Objects.Any(e => e.Name == "One"));
            Assert.AreNotEqual("One", model.Objects.First(e => e.Name == "One").Properties.First().Name);
        }

        private static async Task<WebApiGeneratorModel> GetAnnotationTargetsModel()
        {
            return await BuildModel("files/raml1/annotations-targets.raml");
        }

        private static async Task<WebApiGeneratorModel> GetAnnotationsModel()
        {
            return await BuildModel("files/raml1/annotations.raml");
        }


        private async Task<WebApiGeneratorModel> GetCustomScalarModel()
        {
            return await BuildModel("files/raml1/customscalar.raml");
        }

        private async Task<WebApiGeneratorModel> GetMoviesModel()
        {
            return await BuildModel("files/raml1/movies-v1.raml");
        }

        private async Task<WebApiGeneratorModel> GetMovieTypeModel()
        {
            return await BuildModel("files/raml1/movietype.raml");
        }

        private async Task<WebApiGeneratorModel> GetParametersModel()
        {
            return await BuildModel("files/raml1/parameters.raml");
        }

        private async Task<WebApiGeneratorModel> GetTypeExpressionsModel()
        {
            return await BuildModel("files/raml1/typeexpressions.raml");
        }


        private static async Task<WebApiGeneratorModel> BuildModel(string ramlFile)
        {
            //var fi = new FileInfo(ramlFile);
            var raml = await new AmfParser().Load(ramlFile);
            var model = new WebApiGeneratorService(raml, "TargetNamespace").BuildModel();

            return model;
        }
    }
}