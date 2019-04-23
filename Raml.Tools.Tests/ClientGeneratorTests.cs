using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser;
using Raml.Tools.ClientGenerator;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class ClientGeneratorTests
    {

        [TestMethod]
        public async Task ShouldBuildMethodsWithTheFullPath_WhenEmptyResourceParents()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(5, model.Classes.SelectMany(c => c.Methods).Count());
        }

        [TestMethod]
        public async Task ShouldBuildQueryObjects_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(1, model.QueryObjects.Count());
        }

        [TestMethod]
        public async Task Warnings_WhenBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(48, model.Warnings.Count());
        }

        //[Test, Ignore]
        //public async Task Warnings_WhenTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(60, model.Warnings.Count());
        //}

        //[Test, Ignore]
        //public async Task Warnings_WhenGithub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(60, model.Warnings.Count());
        //}

        [TestMethod]
        public async Task ShouldHaveNoWarnings_WhenCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(0, model.Warnings.Count());
        }

        [TestMethod]
        public async Task ShouldHaveNoWarnings_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(0, model.Warnings.Count());
        }

        [TestMethod]
        public async Task ShouldHaveNoWarnings_WhenFstab()
        {
            var model = await GetFstabGeneratedModel();

            Assert.AreEqual(0, model.Warnings.Count());
        }

        [TestMethod]
        public async Task ShouldGetConcreteImplementationsForDefinitions_WhenFstab()
        {
            var model = await GetFstabGeneratedModel();

            var baseClass = model.Objects.Where(o => o.Name == "Storage").ToArray();
            var implementations = model.Objects.Where(o => o.BaseClass == "Storage").ToArray();
            
            Assert.AreEqual(1, baseClass.Length);
            Assert.AreEqual(4, implementations.Length);
        }

        [TestMethod]
        public async Task ShouldHaveNoWarnings_WhenContacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(0, model.Warnings.Count());
        }

        [TestMethod]
        public async Task ShouldBuildQueryObjectsFromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(1, model.QueryObjects.Count());
            Assert.AreEqual(1, model.Classes.SelectMany(c => c.Methods).Count(m => m.Query != null));
        }

        [TestMethod]
        public async Task ShouldBuildQueryObjectsFromCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(2, model.QueryObjects.Count());
            Assert.AreEqual(2, model.QueryObjects.Values.Count(m => m.Properties.Any()));
        }

        [TestMethod]
        public async Task ShouldBuildQueryObjectsFromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(13, model.QueryObjects.Count());
            Assert.AreEqual(13, model.Classes.SelectMany(c => c.Methods).Count(m => m.Query != null));
        }

        [TestMethod]
        public async Task ShouldBuildMethodsFromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(4, model.Classes.SelectMany(c => c.Methods).Count());
        }

        //[Test, Ignore]
        //public async Task ShouldBuildMethodsFromBox()
        //{
        //    var model = await GetBoxGeneratedModel();
        //    Assert.AreEqual(52, model.Classes.SelectMany(c => c.Methods).Count());
        //}

        //[Test, Ignore]
        //public async Task ShouldBuildMethodsFromLarge()
        //{
        //    var model = await GetLargeGeneratedModel();
        //    Assert.AreEqual(52, model.Classes.SelectMany(c => c.Methods).Count());
        //}

        //[Test, Ignore]
        //public async Task ShouldBuildMethodsFromRegression()
        //{
        //    var model = await GetRegressionGeneratedModel();
        //    Assert.AreEqual(18, model.Classes.SelectMany(c => c.Methods).Count());
        //}


        [TestMethod]
        public async Task ShouldBuildNamespace()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual("RemoteVendingAPI", model.Namespace);
        }

        [TestMethod]
        public async Task ShouldGetRequestObjectsFromResourceTypes_FromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.IsNotNull(model.Classes.First(c => c.Name == "Sales").Methods.First(m => m.Verb == "Post").Parameter);
        }

        [TestMethod]
        public async Task ShouldBuildRequestObjects_FromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(9, model.RequestObjects.Count);
        }

        //[Test, Ignore]
        //public async Task ShouldBuildResponseObjects_FromInstagram()
        //{
        //    var model = await GetInstagramGeneratedModel();
        //    Assert.AreEqual(11, model.ResponseObjects.Count);
        //}

        //[Test, Ignore]
        //public async Task ShouldBuildClasses_FromTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(107, model.Classes.Count());
        //}

        [TestMethod]
        public async Task ShouldBuildClasses_FromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(2, model.Classes.Count());
        }


        [TestMethod]
        public async Task ShouldBuildPropertiosOnClasses_FromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(16, model.Classes.SelectMany(c => c.Properties).Count());
        }

        [TestMethod]
        public async Task ShouldBuildPropertiosOnrOOTClass_FromCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(2, model.Root.Properties.Count());
        }

        //[Test, Ignore]
        //public async Task ShouldBuildPropertiosOnClasses_FromGitHub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(91, model.Classes.SelectMany(c => c.Properties).Count());
        //}

        //[Test, Ignore]
        //public async Task ShouldBuildPropertiosOnClasses_FromTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(91, model.Classes.SelectMany(c => c.Properties).Count());
        //}

        [TestMethod]
        public async Task ShouldBuildPropertiosOnClasses_FromContacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(2, model.Classes.SelectMany(c => c.Properties).Count());
        }


        [TestMethod]
        public async Task ShouldBuildClasses_FromContacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(4, model.Classes.Count());
        }

        //[Test, Ignore]
        //public async Task ShouldBuildClasses_FromGithub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(108, model.Classes.Count());
        //}

        [TestMethod]
        public async Task ShouldBuildClasses_FromCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(3, model.Classes.Count());
        }

        [TestMethod]
        public async Task ShouldBuildClasses_FromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(26, model.Classes.Count());
        }

        [TestMethod]
        public async Task ShouldExcludeOptionsFromMethods()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(0, model.Classes.SelectMany(c => c.Methods).Count(m => m.Verb == "Options"));
        }

        [TestMethod]
        public async Task ShouldIncludePatchFromMethods()
        {
            var model = await GetCongoGeneratedModel();
            Assert.IsTrue(model.Classes.SelectMany(c => c.Methods).Count(m => m.Verb == "Patch") > 0);
        }

        //[Test, Ignore]
        //public async Task ShouldBuildResponseObjects_FromTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(62, model.ResponseObjects.Count);
        //}

        //[Test, Ignore]
        //public async Task ShouldBuildRequestObjects_FromGitHub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(47, model.RequestObjects.Count);
        //}

        //[Test, Ignore]
        //public async Task ShouldAvoidObjectsWithFrameworkNames()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.IsFalse(model.Objects.Any(o => o.Name == "Get"));
        //}

        //[Test, Ignore]
        //public async Task ShouldBuildResponseObjects_FromGitHub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(79, model.ResponseObjects.Count);
        //}

        [TestMethod]
        public async Task ShouldParseArrays()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Sales"), model.Objects.First(o => o.Name == "GetSales").Properties[1].Type);
        }

        [TestMethod]
        public async Task ShouldParseCommonObjects()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual("Exchange", model.SchemaObjects["exchange"].Name);
            Assert.AreEqual(3, model.SchemaObjects["exchange"].Properties.Count);
        }

        [TestMethod]
        public async Task ShouldParse_Congo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(11, model.Classes.SelectMany(c => c.Methods).Count());
            Assert.AreEqual(13, model.Objects.Count());
        }

        //[Test, Ignore]
        //public async Task ShouldBuildUriParametersObjects_FromTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(95, model.UriParameterObjects.Count());
        //}

        [TestMethod]
        public async Task ShouldBuildClasses_FromDars()
        {
            var model = await GetDarsGeneratedModel();
            Assert.AreEqual(2, model.SchemaObjects.Count());
        }

        [TestMethod]
        public async Task ShouldBuildClasses_FromDarsWithParams()
        {
            var model = await GetDarsWithParamsGeneratedModel();
            Assert.AreEqual(2, model.SchemaObjects.Count());
        }


        [TestMethod]
        public async Task ShouldBuildClasses_FromEpi()
        {
            var model = await GetEpiGeneratedModel();
            Assert.AreEqual(2, model.Classes.Count());
        }

        [TestMethod]
        public async Task ShouldBuildClasses_FromFoo()
        {
            var model = await GetFooGeneratedModel();
            Assert.AreEqual(2, model.Classes.Count());
        }

        [TestMethod]
        public async void Should_Name_Schemas_Using_Keys()
        {
            var model = await GetSchemaTestsGeneratedModel();
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Thing"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Things"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ThingResult"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ThingRequest"));
        }

        [TestMethod]
        public async void Should_Generate_Properties_When_Movies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(88, model.Objects.Sum(c => c.Properties.Count));
        }

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Box()
        //{
        //    var model = await GetBoxGeneratedModel();
        //    Assert.AreEqual(78, model.Objects.Sum(c => c.Properties.Count));
        //}

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Congo()
        //{
        //    var model = await GetCongoGeneratedModel();
        //    Assert.AreEqual(42, model.Objects.Sum(c => c.Properties.Count));
        //}

        [TestMethod]
        public async void Should_Generate_Properties_When_Contacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(5, model.Objects.Sum(c => c.Properties.Count));
        }

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_GitHub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    Assert.AreEqual(1006, model.Objects.Sum(c => c.Properties.Count));
        //}

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Instagram()
        //{
        //    var model = await GetInstagramGeneratedModel();
        //    Assert.AreEqual(128, model.Objects.Sum(c => c.Properties.Count));
        //}

        [TestMethod]
        public async void Should_Generate_Properties_When_Large()
        {
            var model = await GetLargeGeneratedModel();
            Assert.AreEqual(77, model.Objects.Sum(c => c.Properties.Count));
        }

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Regression()
        //{
        //    var model = await GetRegressionGeneratedModel();
        //    Assert.AreEqual(144, model.Objects.Sum(c => c.Properties.Count));
        //}

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Test()
        //{
        //    var model = await GetTestGeneratedModel();
        //    Assert.AreEqual(39, model.Objects.Sum(c => c.Properties.Count));
        //}

        //[Test, Ignore]
        //public async void Should_Generate_Properties_When_Twitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    Assert.AreEqual(673, model.Objects.Sum(c => c.Properties.Count));
        //}

        //[Test, Ignore]
        //public async Task Should_Generate_Valid_XML_Comments_WhenGithub()
        //{
        //    var model = await GetGitHubGeneratedModel();
        //    var xmlDoc = new XmlDocument();
        //    foreach (var method in model.Classes.SelectMany(c => c.Methods))
        //    {
        //        var xmlComment = GetXml(method.XmlComment);
        //        var xmlSimpleComment = GetXml(method.XmlSimpleComment);
        //        Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlComment));
        //        Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlSimpleComment));
        //    }
        //}

        //[Test, Ignore]
        //public async Task Should_Generate_Valid_XML_Comments_WhenTwitter()
        //{
        //    var model = await GetTwitterGeneratedModel();
        //    var xmlDoc = new XmlDocument();
        //    foreach (var method in model.Classes.SelectMany(c => c.Methods))
        //    {
        //        var xmlComment = GetXml(method.XmlComment);
        //        var xmlSimpleComment = GetXml(method.XmlSimpleComment);
        //        Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlComment));
        //        Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlSimpleComment));
        //    }
        //}

        [TestMethod]
        public async Task Should_Generate_Valid_XML_Comments_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            var xmlDoc = new XmlDocument();
            foreach (var method in model.Classes.SelectMany(c => c.Methods))
            {
                var xmlComment = GetXml(method.XmlComment);
                var xmlSimpleComment = GetXml(method.XmlSimpleComment);
                xmlDoc.LoadXml(xmlComment);
                xmlDoc.LoadXml(xmlSimpleComment);
            }
        }

        [TestMethod]
        public async Task ShouldGenerateEnums_WhenFstab()
        {
            var model = await GetFstabGeneratedModel();
            Assert.AreEqual(4, model.Enums.Count());
            Assert.AreEqual(5, model.Objects.Sum(o => o.Properties.Count(p => p.IsEnum)));
        }

        [TestMethod]
        public async Task ShouldGenerateModels_WhenExternalRefs()
        {
            var model = await GetExternalRefsGeneratedModel();
            Assert.AreEqual(4, model.Objects.Count());
        }

        [TestMethod]
        public async Task ShouldGenerateDifferentModels_WhenDifferentPropertiesWithSameName()
        {
            var model = await GetSameNameGeneratedModel();
            Assert.AreEqual(2, model.Objects.Count(o => o.Name.ToLowerInvariant().Contains("director")));
        }

        [TestMethod]
        public async Task ShouldGenerateProperties_Issue17()
        {
            var model = await GetIssue17GeneratedModel();
            Assert.IsTrue(model.SchemaObjects.Where(o => o.Key.EndsWith("Content")).All(o => o.Value.Properties.Count == 4));
        }

        [TestMethod]
        public async Task ShouldNotDuplicateClasses()
        {
            var model = await GetDuplicationGeneratedModel();
            Assert.AreEqual(2, model.Objects.Count(o => o.Name.StartsWith("User")));
            Assert.AreEqual(1, model.Objects.Count(o => o.Name.Equals("User")));
        }

        [TestMethod]
        public async Task ShouldGenerateMethodsForPatch()
        {
            var model = await GetPatchGeneratedModel();
            Assert.AreEqual(1, model.Classes.Sum(o => o.Methods.Count(m => m.Verb == "Patch")));
            Assert.AreEqual(2, model.Objects.Count(o => o.Name.Contains("Patch")));
        }

        //[Test, Ignore]
        //public async Task ShouldAcceptDigestSecurity()
        //{
        //    var model = await BuildModel("files/raml1/digest.raml");
        //    Assert.IsNotNull(model.Security);
        //}

        [TestMethod]
        public async Task ShouldGenerateRootResource()
        {
            var model = await GetRootGeneratedModel();
            Assert.IsTrue(model.Classes.Any(c => c.Name == "RootUrl"));
            Assert.AreEqual(3, model.Classes.First(c => c.Name == "RootUrl").Methods.Count);
        }

        [TestMethod]
        public async Task ShouldAcceptWithoutBaseUri()
        {
            var model = await BuildModel("files/raml1/baseUri.raml");
            Assert.IsTrue(string.IsNullOrEmpty(model.BaseUri));
        }

        //[Test, Ignore] // JSON.NET issue
        //public async Task ShouldHandleJsonSchemaRecursiveTree()
        //{
        //    var raml = await new RamlParser().LoadAsync("files/tree-issue63.raml");
        //    var model = new ClientGeneratorService(raml, "test", "TargetNamespace").BuildModel();

        //    Assert.IsNotNull(model);
        //    Assert.AreEqual(1, model.Objects.Count());
        //}

        private static string GetXml(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return comment;

            return "<root>" + comment.Replace("///", string.Empty).Replace("\\\"", "\"") + "</root>";
        }

        private static async Task<ClientGeneratorModel> GetTestGeneratedModel()
        {
            return await BuildModel("files/test.raml");
        }

        private static async Task<ClientGeneratorModel> GetBoxGeneratedModel()
        {
            return await BuildModel("files/box.raml");
        }


        private async Task<ClientGeneratorModel> GetLargeGeneratedModel()
        {
            return await BuildModel("files/large.raml");
        }

        private async Task<ClientGeneratorModel> GetRegressionGeneratedModel()
        {
            return await BuildModel("files/regression.raml");
        }

        private async Task<ClientGeneratorModel> GetCongoGeneratedModel()
        {
            return await BuildModel("files/congo-drones-5-f.raml");
        }

        private async Task<ClientGeneratorModel> GetInstagramGeneratedModel()
        {
            return await BuildModel("files/instagram.raml");
        }

        private async Task<ClientGeneratorModel> GetTwitterGeneratedModel()
        {
            return await BuildModel("files/twitter.raml");
        }

        private async Task<ClientGeneratorModel> GetGitHubGeneratedModel()
        {
            return await BuildModel("files/github.raml");
        }

        private async Task<ClientGeneratorModel> GetContactsGeneratedModel()
        {
            return await BuildModel("files/contacts.raml");
        }

        private static async Task<ClientGeneratorModel> GetMoviesGeneratedModel()
        {
            return await BuildModel("files/movies.raml");
        }

        private static async Task<ClientGeneratorModel> GetFstabGeneratedModel()
        {
            return await BuildModel("files/fstab.raml");
        }

        private static async Task<ClientGeneratorModel> GetDarsGeneratedModel()
        {
            return await BuildModel("files/dars.raml");
        }

        private static async Task<ClientGeneratorModel> GetDarsWithParamsGeneratedModel()
        {
            return await BuildModel("files/darsparam.raml");
        }

        private static async Task<ClientGeneratorModel> GetEpiGeneratedModel()
        {
            return await BuildModel("files/epi.raml");
        }

        private static async Task<ClientGeneratorModel> GetFooGeneratedModel()
        {
            return await BuildModel("files/foo.raml");
        }

        private static async Task<ClientGeneratorModel> GetSchemaTestsGeneratedModel()
        {
            return await BuildModel("files/schematests.raml");
        }


        private static async Task<ClientGeneratorModel> GetExternalRefsGeneratedModel()
        {
            return await BuildModel("files/external-refs.raml");
        }

        private static async Task<ClientGeneratorModel> GetSameNameGeneratedModel()
        {
            return await BuildModel("files/same-name-dif-obj.raml");
        }

        private static async Task<ClientGeneratorModel> GetIssue17GeneratedModel()
        {
            return await BuildModel("files/issue17.raml");
        }

        private static async Task<ClientGeneratorModel> GetDuplicationGeneratedModel()
        {
            return await BuildModel("files/duplication.raml");
        }

        private static async Task<ClientGeneratorModel> GetPatchGeneratedModel()
        {
            return await BuildModel("files/patch.raml");
        }

        private static async Task<ClientGeneratorModel> BuildModel(string ramlFile)
        {
            var fi = new FileInfo(ramlFile);
            var raml = await new RamlParser().LoadAsync(fi.FullName);
            var model = new ClientGeneratorService(raml, "test", "TargetNamespace").BuildModel();

            return model;
        }

        private static async Task<ClientGeneratorModel> GetRootGeneratedModel()
        {
            var raml = await new RamlParser().LoadAsync("files/root.raml");
            var model = new ClientGeneratorService(raml, "Root", "TargetNamespace").BuildModel();

            return model;
    }

    }
}