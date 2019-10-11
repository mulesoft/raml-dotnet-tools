using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser;
using Raml.Tools.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class WebApiGeneratorTests
    {
        [TestMethod]
        public async void Should_Generate_Controller_Objects_When_Movies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [TestMethod]
        public async void Should_Generate_Controller_Objects_When_Box()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(10, model.Controllers.Count());
        }

        [TestMethod]
        public async void Should_Generate_Controller_Objects_When_Congo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(2, model.Controllers.Count());
        }

        [TestMethod]
        public async void Should_Generate_Controller_Objects_When_Contacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(2, model.Controllers.Count());
        }


        //[Test, Ignore]
        //public async void Should_Generate_Controller_Objects_When_GitHub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(17, model.Controllers.Count());
        //}


        //[Test, Ignore]
        //public async void Should_Generate_Controller_Objects_When_Instagram()
        //{
        //    var model = await GetInstagramGeneratedModel();
        //    Assert.AreEqual(7, model.Controllers.Count());
        //}

        [TestMethod]
        public async void Should_Generate_Controller_Objects_When_Large()
        {
            var model = await GetLargeGeneratedModel();
            Assert.AreEqual(10, model.Controllers.Count());
        }


        //[Test, Ignore]
        //public async void Should_Generate_Controller_Objects_When_Regression()
        //{
        //    var model = await GetRegressionGeneratedModel();
        //    Assert.AreEqual(2, model.Controllers.Count());
        //}

        [TestMethod]
        public async void Should_Generate_Controller_Objects_When_Test()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(2, model.Controllers.Count());
        }


        //[Test, Ignore]
        //public async void Should_Generate_Controller_Objects_When_Twitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(16, model.Controllers.Count());
        //}

        [TestMethod]
        public async void Should_Remove_Not_Used_Objects_Dars()
        {
            var model = await GetDarsGeneratedModel();
            Assert.AreEqual(2, model.Objects.Count());
        }


        //[Test, Ignore]
        //public async void Should_Not_Remove_Used_Objects_Twitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.IsTrue(model.Objects.Any(o => o.Name == "ContainedWithin"));
        //    Assert.AreEqual(62, model.Objects.Count());
        //}

        [TestMethod]
        public async void Should_Name_Schemas_Using_Keys()
        {
            var model = await GetSchemaTestsGeneratedModel();
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Thing"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Things"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ThingResult"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ThingRequest"));
            Assert.AreEqual(5, model.Objects.Count());
        }

        [TestMethod]
        public async void Should_Generate_Properties_When_Movies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(82, model.Objects.Sum(c => c.Properties.Count));
        }

        [TestMethod]
        public async void Should_Generate_Properties_When_Box()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(25, model.Objects.Sum(c => c.Properties.Count));
        }

        [TestMethod]
        public async void Should_Generate_Properties_When_Congo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(29, model.Objects.Sum(c => c.Properties.Count));
        }

        [TestMethod]
        public async void Should_Generate_Properties_When_Contacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(3, model.Objects.Sum(c => c.Properties.Count));
        }


        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_GitHub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(692, model.Objects.Sum(c => c.Properties.Count));
        //}


        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Instagram()
        //{
        //    var model = await GetInstagramGeneratedModel();
        //    Assert.AreEqual(35, model.Objects.Sum(c => c.Properties.Count));
        //}

        [TestMethod]
        public async void Should_Generate_Properties_When_Large()
        {
            var model = await GetLargeGeneratedModel();
            Assert.AreEqual(24, model.Objects.Sum(c => c.Properties.Count));
        }


        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Regression()
        //{
        //    var model = await GetRegressionGeneratedModel();
        //    Assert.AreEqual(130, model.Objects.Sum(c => c.Properties.Count));
        //}


        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Test()
        //{
        //    var model = await GetTestGeneratedModel();
        //    Assert.AreEqual(36, model.Objects.Sum(c => c.Properties.Count));
        //}


        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Twitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(348, model.Objects.Sum(c => c.Properties.Count));
        //}


        //[Test, Ignore]
        //public async Task Should_Generate_Valid_XML_Comments_WhenGithub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    var xmlDoc = new XmlDocument();
        //    foreach (var method in model.Controllers.SelectMany(c => c.Methods))
        //    {
        //        var xmlComment = GetXml(method.XmlComment);
        //        Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlComment));
        //    }
        //}


        //[Test, Ignore]
        //public async Task Should_Generate_Valid_XML_Comments_WhenTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    var xmlDoc = new XmlDocument();
        //    foreach (var method in model.Controllers.SelectMany(c => c.Methods))
        //    {
        //        var xmlComment = GetXml(method.XmlComment);
        //        Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlComment));
        //    }
        //}

        [TestMethod]
        public async Task Should_Generate_Valid_XML_Comments_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            var xmlDoc = new XmlDocument();
            foreach (var method in model.Controllers.SelectMany(c => c.Methods))
            {
                var xmlComment = GetXml(method.XmlComment);
                xmlDoc.LoadXml(xmlComment);
            }
        }


        //[Test, Ignore]
        //public async Task Should_Link_Response_And_Request_With_Types_When_Orders_XML()
        //{
        //    var model = await GetOrdersXmlGeneratedModel();
        //    Assert.AreEqual("PurchaseOrderType", model.Controllers.First().Methods.First(m => m.Verb == "Get").ReturnType);
        //    Assert.AreEqual("PurchaseOrderType", model.Controllers.First().Methods.First(m => m.Verb == "Post").Parameter.Type);
        //}



        [TestMethod]
        public async Task ShouldGenerateProperties_Issue17()
        {
            var model = await GetIssue17GeneratedModel();
            Assert.IsTrue(model.Objects.All(o => o.Properties.Count == 4));
        }

        [TestMethod]
        public async Task ShouldParseSchemas_Issue13()
        {
            var model = await GetIssue13GeneratedModel();

            Assert.AreEqual(15, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldNotReuseModelIfDifferent_Issue25()
        {
            var model = await GetIssue25GeneratedModel();

            Assert.AreEqual(2, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldInheritUriParametersType_Issue23()
        {
            var model = await GetIssue23GeneratedModel();
            Assert.AreEqual("int", model.Controllers.First().Methods.First(m => m.Name == "GetById").UriParameters.First().Type);
            Assert.AreEqual("int", model.Controllers.First().Methods.First(m => m.Name == "GetHistory").UriParameters.First().Type);
        }

        [TestMethod]
        public async Task ShouldNotRemoveAdditionalPropertiesProperty()
        {
            var model = await GetAdditionalPropertiesGeneratedModel();
            Assert.IsTrue(model.Objects.Any(o => o.Properties.Any(p => p.IsAdditionalProperties)));
        }

        [TestMethod]
        public async Task NestedAdditionalProperties()
        {
            var raml = await new RamlParser().LoadAsync("files/additionalprops-nested.raml");
            var model = new WebApiGeneratorService(raml, "TargetNamespace").BuildModel();
            Assert.AreEqual(2, model.Objects.Count());
        }

        [TestMethod]
        public async Task NestedAdditionalProperties_v4Schema()
        {
            var raml = await new RamlParser().LoadAsync("files/additionalprops-nested-v4.raml");
            var model = new WebApiGeneratorService(raml, "TargetNamespace").BuildModel();
            Assert.AreEqual(2, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldParseTratisWith2Responses()
        {
            var model = await GetIssue37GeneratedModel();
            Assert.AreEqual(3, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldGenerateRootResource()
        {
            var model = await GetRootGeneratedModel();
            Assert.IsTrue(model.Controllers.Any(c => c.Name == "RootUrl"));
            Assert.AreEqual(3, model.Controllers.First(c => c.Name == "RootUrl").Methods.Count);
        }

        [TestMethod]
        public async Task ShouldAccept3LevelNestingSchema()
        {
            var raml = await new RamlParser().LoadAsync("files/level3nest.raml");
            var model = new WebApiGeneratorService(raml, "TestNs").BuildModel();

            Assert.AreEqual(3, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldAccept3LevelNestingSchemaWithArray()
        {
            var raml = await new RamlParser().LoadAsync("files/level3nest-array.raml");
            var model = new WebApiGeneratorService(raml, "TestNs").BuildModel();

            Assert.AreEqual(3, model.Objects.Count());
        }

        [TestMethod]

        public async Task ShouldAccept3LevelNestingVersion3Schema()
        {
            var raml = await new RamlParser().LoadAsync("files/level3nest-v3.raml");
            var model = new WebApiGeneratorService(raml, "TestNs").BuildModel();

            Assert.AreEqual(3, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldAcceptReferenceToRAMLSchemaKey()
        {
            var raml = await new RamlParser().LoadAsync("files/issue59.raml");
            var model = new WebApiGeneratorService(raml, "TestNs").BuildModel();

            Assert.AreEqual(17, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldAcceptReferenceToRAMLSchemaKey_issue64()
        {
            var raml = await new RamlParser().LoadAsync("files/issue64.raml");
            var model = new WebApiGeneratorService(raml, "TestNs").BuildModel();
            Assert.AreEqual(3, model.Objects.Count());
            Assert.AreNotEqual("string", model.Controllers.First().Methods.First().Parameter.Type);
        }

        [TestMethod]
        public async Task ShouldWorkIncludeWithIncludes()
        {
            var raml = await new RamlParser().LoadAsync("files/included-files.raml");
            var model = new WebApiGeneratorService(raml, "TestNs").BuildModel();
            Assert.AreEqual(2, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldParseTraitWithResponseAndBodyResponse()
        {
            var model = await BuildModel("files/trait-multiple-response.raml");
            Assert.AreEqual(3, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldParseTraitsWithResponses()
        {
            var model = await BuildModel("files/traits-response.raml");
            Assert.AreEqual(5, model.Objects.Count());
            Assert.AreEqual("ContactsGetOKResponseContent", model.Controllers.First(c => c.PrefixUri == "contacts").Methods.First(m => m.Url == "").ReturnType);
            Assert.AreEqual("ContactsIdGetBadRequestResponseContent", model.Controllers.First(c => c.PrefixUri == "contacts").Methods.First(m => m.Url == "{id}").ReturnType);
            Assert.AreEqual("MultipleTestGet", model.Controllers.First(c => c.PrefixUri == "test").Methods.First(m => m.Url == "").ReturnType);
        }

        [TestMethod]
        public async Task ShouldHandleRepeatedNamesInEnums()
        {
            var model = await BuildModel("files/enums-repeated-names.raml");
            Assert.AreEqual(3, model.Objects.Count());
            Assert.AreEqual(3, model.Enums.Count());
            Assert.AreEqual("Color", model.Objects.First(o => o.Name == "Things").Properties.First(p => p.IsEnum).Type);
            Assert.AreEqual("Color0", model.Objects.First(o => o.Name == "Thing").Properties.First(p => p.IsEnum).Type);
            Assert.AreEqual("Color1", model.Objects.First(o => o.Name == "ThingResult").Properties.First(p => p.IsEnum).Type);
        }

        [TestMethod]
        public async Task ShouldHandleRepeatedNamesInEnums_Schema_v4()
        {
            var model = await BuildModel("files/enums-repeated-names-v4.raml");
            Assert.AreEqual(3, model.Objects.Count());
            Assert.AreEqual(3, model.Enums.Count());
            Assert.AreEqual("Color", model.Objects.First(o => o.Name == "Things").Properties.First(p => p.IsEnum).Type);
            Assert.AreEqual("Color0", model.Objects.First(o => o.Name == "Thing").Properties.First(p => p.IsEnum).Type);
            Assert.AreEqual("Color1", model.Objects.First(o => o.Name == "ThingResult").Properties.First(p => p.IsEnum).Type);
        }

        [TestMethod]
        public async Task ShouldHandleSimilarSchemas()
        {
            var model = await BuildModel("files/similar-schemas-ignored.raml");
            Assert.AreEqual(2, model.Objects.Count());
            Assert.AreEqual("Thing", model.Controllers.First(o => o.Name == "Things").Methods.First().ReturnType);
            Assert.AreEqual("Thingy", model.Controllers.First(o => o.Name == "Thingys").Methods.First().ReturnType);
        }

        private static string GetXml(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return comment;

            return "<root>" + comment.Replace("///", string.Empty).Replace("\\\"", "\"") + "</root>";
        }

		private static async Task<WebApiGeneratorModel> GetTestGeneratedModel()
		{
            return await BuildModel("files/test.raml");
        }

		private static async Task<WebApiGeneratorModel> GetBoxGeneratedModel()
		{
            return await BuildModel("files/box.raml");
        }


        private async Task<WebApiGeneratorModel> GetLargeGeneratedModel()
        {
            return await BuildModel("files/large.raml");
		}

        private async Task<WebApiGeneratorModel> GetRegressionGeneratedModel()
        {
            return await BuildModel("files/regression.raml");
		}

		private async Task<WebApiGeneratorModel> GetCongoGeneratedModel()
		{
		    return await BuildModel("files/congo-drones-5-f.raml");
		}

        private async Task<WebApiGeneratorModel> GetInstagramGeneratedModel()
		{
            return await BuildModel("files/instagram.raml");
		}

		private async Task<WebApiGeneratorModel> GetTwitterGeneratedModel()
		{
            return await BuildModel("files/twitter.raml");
		}

		private async Task<WebApiGeneratorModel> GetGitHubGeneratedModel()
		{
            return await BuildModel("files/github.raml");
		}

		private async Task<WebApiGeneratorModel> GetContactsGeneratedModel()
		{
            return await BuildModel("files/contacts.raml");
		}

		private static async Task<WebApiGeneratorModel> GetMoviesGeneratedModel()
		{
            return await BuildModel("files/movies.raml");
        }

		private static async Task<WebApiGeneratorModel> GetDarsGeneratedModel()
		{
			return await BuildModel("files/dars.raml");
        }

		private static async Task<WebApiGeneratorModel> GetSchemaTestsGeneratedModel()
		{
			return await BuildModel("files/schematests.raml");
        }


        private static async Task<WebApiGeneratorModel> GetOrdersXmlGeneratedModel()
        {
            return await BuildModel("files/ordersXml.raml");
        }
	
        private static async Task<WebApiGeneratorModel> GetIssue17GeneratedModel()
        {
            return await BuildModel("files/issue17.raml");
        }

        private static async Task<WebApiGeneratorModel> GetIssue13GeneratedModel()
        {
            return await BuildModel("files/issue13.raml");
        }

        private static async Task<WebApiGeneratorModel> GetIssue25GeneratedModel()
        {
            return await BuildModel("files/issue25.raml");
        }

        private static async Task<WebApiGeneratorModel> GetIssue23GeneratedModel()
        {
            return await BuildModel("files/issue23.raml");
        }

        private static async Task<WebApiGeneratorModel> GetAdditionalPropertiesGeneratedModel()
        {
            return await BuildModel("files/additionalprops.raml");
        }

        private static async Task<WebApiGeneratorModel> GetIssue37GeneratedModel()
        {
            return await BuildModel("files/issue37.raml");
        }

        private static async Task<WebApiGeneratorModel> BuildModel(string ramlFile)
        {
            var fi = new FileInfo(ramlFile);
            var raml = await new RamlParser().LoadAsync(fi.FullName);
            return new WebApiGeneratorService(raml, "TestNs").BuildModel();
        }

        private static async Task<WebApiGeneratorModel> GetRootGeneratedModel()
        {
            var raml = await new RamlParser().LoadAsync("files/root.raml");
            var model = new WebApiGeneratorService(raml, "TargetNamespace").BuildModel();

            return model;
    	}
    }
}