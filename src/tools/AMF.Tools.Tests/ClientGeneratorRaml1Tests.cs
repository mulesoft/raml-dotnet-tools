using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.ClientGenerator;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class ClientGeneratorRaml1Tests
    {
        public int TestCount = 0;
        private void IncrementTestCount()
        {
            TestCount++;
        }

        [Test]
        public async Task ShouldBuild_WhenCustomScalar()
        {
            var model = await GetCustomScalarModel();
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldBuildUriParameter_WhenCustomScalar()
        {
            var model = await GetCustomScalarModel();
            //Assert.IsNotNull(model.Objects.First(o => o.Name == "CustomDate"));
            //Assert.IsNotNull(model.Objects.First(o => o.Name == "Mydate"));
            //Assert.IsTrue(model.Objects.First(o => o.Name == "CustomDate").IsScalar);
            Assert.AreEqual("DateTime", model.Classes.First().Methods.First().UriParameters.First().Type);
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
        public async Task ShouldBuildArrayTypes()
        {
            var model = await GetArraysModel();
            
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ArrayOfPerson"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ArrayOfInt"));
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("int"), model.Objects.First(o => o.Name == "ArrayOfInt").Type);
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ArrayOfObject"));
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Person"), model.Objects.First(o => o.Name == "ArrayOfPerson").Type);

            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Person"), model.Objects
                .First(o => o.Name == "TypeThatHasArrayProperty")
                .Properties
                .First(p => p.Name == "Persons").Type);

            Assert.IsTrue(model.Objects.Any(o => o.Name == "Items"));
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Items"), model.Objects.First(o => o.Name == "ArrayOfObject").Type);

            Assert.AreEqual(6, model.Objects.Count());
        }

        [Test]
        public async Task ShouldBuild_WhenParameters()
        {
            var model = await GetParametersModel();
            Assert.IsNotNull(model);
        }

        //[Test]
        //public async Task ShouldHandleTypeExpressions()
        //{
        //    var model = await GetTypeExpressionsModel();
        //    //Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Movie"), model.Classes.First().Methods.First(m => m.Verb == "Get").OkReturnType);
        //    Assert.AreEqual("string", model.Classes.First().Methods.First(m => m.Verb == "Put").Parameter.Type);
        //    Assert.AreEqual("string", model.Classes.First().Methods.First(m => m.Verb == "Post").Parameter.Type);
        //}

        [Test]
        public async Task ShouldHandleInlinedTypes()
        {
            var model = await BuildModel("files/raml1/inlinetype.raml");
            Assert.AreEqual(2, model.Objects.Count());
            //Assert.AreEqual("UsersGetOKResponseContent", model.Classes.First().Methods.First(m => m.Verb == "Get").OkReturnType);
            //Assert.AreEqual("UsersPostRequestContent", model.Classes.First().Methods.First(m => m.Verb == "Post").Parameter.Type);
        }

        [Test]
        public async Task ShouldHandleEnums()
        {
            var model = await BuildModel("files/raml1/enums.raml");
            Assert.AreEqual(3, model.Enums.Count());
            Assert.AreEqual("E1_year", model.Enums.First(e => e.Name == "Something").Values.First().Name);
            Assert.AreEqual("two years", model.Enums.First(e => e.Name == "Something").Values.Last().OriginalName);
            Assert.AreEqual("two_years", model.Enums.First(e => e.Name == "Something").Values.Last().Name);
            Assert.AreEqual("Something", model.Objects.First(e => e.Name == "Person").Properties.First(p => p.Name == "Something").Type);
            Assert.AreEqual("Country", model.Objects.First(e => e.Name == "Person").Properties.First(p => p.Name == "Country").Type);
            Assert.AreEqual("Size", model.Objects.First(e => e.Name == "Person").Properties.First(p => p.Name == "Size").Type);
            Assert.AreEqual("usa", model.Enums.First(e => e.Name == "Country").Values.First().Name);
        }

        [Test]
        public async Task ShouldHandleShortcutsSyntacticSugar()
        {
            var model = await BuildModel("files/raml1/shortcuts.raml");
            Assert.AreEqual(2, model.Objects.Count());
            //Assert.AreEqual(3, model.Objects.First(o => o.Name == "Person").Properties.Count);
        }

        [Test]
        public async Task ShouldHandleArrayItemAsScalar()
        {
            var model = await BuildModel("files/raml1/array-scalar-item.raml");
            Assert.IsNotNull(model);
        }

        [Test]
        public async Task ShouldHandleArrayAsExpression()
        {
            var model = await BuildModel("files/raml1/array-type-expression.raml");
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("string"), model.Objects.First().Properties.First().Type);
        }

        private async Task<ClientGeneratorModel> GetAnnotationTargetsModel()
        {
            return await BuildModel("files/raml1/annotations-targets.raml");
        }

        private async Task<ClientGeneratorModel> GetAnnotationsModel()
        {
            return await BuildModel("files/raml1/annotations.raml");
        }


        private async Task<ClientGeneratorModel> GetCustomScalarModel()
        {
            return await BuildModel("files/raml1/customscalar.raml");
        }

        private async Task<ClientGeneratorModel> GetMoviesModel()
        {
            return await BuildModel("files/raml1/movies-v1.raml");
        }

        private async Task<ClientGeneratorModel> GetMovieTypeModel()
        {
            return await BuildModel("files/raml1/movietype.raml");
        }

        private async Task<ClientGeneratorModel> GetParametersModel()
        {
            return await BuildModel("files/raml1/parameters.raml");
        }

        private async Task<ClientGeneratorModel> GetTypeExpressionsModel()
        {
            return await BuildModel("files/raml1/typeexpressions.raml");
        }

        private async Task<ClientGeneratorModel> GetMapsModel()
        {
            return await BuildModel("files/raml1/maps.raml");
        }

        private async Task<ClientGeneratorModel> GetArraysModel()
        {
            return await BuildModel("files/raml1/arrays.raml");
        }

        private async Task<ClientGeneratorModel> BuildModel(string ramlFile)
        {
            IncrementTestCount();
            var fi = new FileInfo(ramlFile);
            var raml = await new AmfParser().Load(fi.FullName);
            var model = new ClientGeneratorService(raml, "test", "TargetNamespace", "TargetNamespace.Models").BuildModel();

            return model;
        }
    }
}