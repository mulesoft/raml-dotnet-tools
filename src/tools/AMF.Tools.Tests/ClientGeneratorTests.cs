using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.ClientGenerator;
using System.Linq;
using System.Threading.Tasks;
using System.Xml;
using AMF.Tools.Core;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class ClientGeneratorTests
    {
        public int TestCount = 0;
        private void IncrementTestCount()
        {
            TestCount++;
        }

        [Test]
        public async Task ShouldBuildMethodsWithTheFullPath_WhenEmptyResourceParents()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(5, model.Classes.SelectMany(c => c.Methods).Count());
        }

        [Test]
        public async Task ShouldBuildQueryObjects_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(1, model.QueryObjects.Count());
        }

        [Test]
        public async Task Warnings_WhenBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(48, model.Warnings.Count());
        }

        [Test]
        public async Task ShouldHaveNoWarnings_WhenCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(0, model.Warnings.Count());
        }

        [Test]
        public async Task ShouldHaveNoWarnings_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(0, model.Warnings.Count());
        }

        [Test]
        public async Task ShouldHaveNoWarnings_WhenFstab()
        {
            var model = await GetFstabGeneratedModel();

            Assert.AreEqual(0, model.Warnings.Count());
        }

        [Test]
        public async Task ShouldGetConcreteImplementationsForDefinitions_WhenFstab()
        {
            var model = await GetFstabGeneratedModel();

            var baseClass = model.Objects.Where(o => o.Name == "Storage").ToArray();
            var implementations = model.Objects.Where(o => o.BaseClass == "Storage").ToArray();
            
            Assert.AreEqual(1, baseClass.Length);
            Assert.AreEqual(4, implementations.Length);
        }

        [Test]
        public async Task ShouldHaveNoWarnings_WhenContacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(0, model.Warnings.Count());
        }

        [Test]
        public async Task ShouldBuildQueryObjectsFromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(1, model.QueryObjects.Count());
            Assert.AreEqual(1, model.Classes.SelectMany(c => c.Methods).Count(m => m.Query != null));
        }

        [Test]
        public async Task ShouldBuildQueryObjectsFromCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(2, model.QueryObjects.Count());
            Assert.AreEqual(2, model.QueryObjects.Values.Count(m => m.Properties.Any()));
        }

        [Test]
        public async Task ShouldBuildQueryObjectsFromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(13, model.QueryObjects.Count());
            Assert.AreEqual(13, model.Classes.SelectMany(c => c.Methods).Count(m => m.Query != null));
        }

        [Test]
        public async Task ShouldBuildMethodsFromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(4, model.Classes.SelectMany(c => c.Methods).Count());
        }

        [Test]
        public async Task ShouldBuildNamespace()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual("RemoteVendingAPI", model.BaseNamespace);
        }

        [Test]
        public async Task ShouldGetRequestObjectsFromResourceTypes_FromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.IsNotNull(model.Classes.First(c => c.Name == "Sales").Methods.First(m => m.Verb == "Post").Parameter);
        }

        [Test]
        public async Task ShouldBuildRequestObjects_FromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(9, model.RequestObjects.Count);
        }

        [Test]
        public async Task ShouldBuildClasses_FromTest()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(2, model.Classes.Count());
        }


        [Test]
        public async Task ShouldBuildPropertiosOnClasses_FromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(16, model.Classes.SelectMany(c => c.Properties).Count());
        }

        [Test]
        public async Task ShouldBuildPropertiosOnrOOTClass_FromCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(2, model.Root.Properties.Count());
        }

        [Test]
        public async Task ShouldBuildPropertiosOnClasses_FromContacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(2, model.Classes.SelectMany(c => c.Properties).Count());
        }


        [Test]
        public async Task ShouldBuildClasses_FromContacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(4, model.Classes.Count());
        }

        [Test]
        public async Task ShouldBuildClasses_FromCongo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(3, model.Classes.Count());
        }

        [Test]
        public async Task ShouldBuildClasses_FromBox()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(26, model.Classes.Count());
        }

        [Test]
        public async Task ShouldExcludeOptionsFromMethods()
        {
            var model = await GetBoxGeneratedModel();
            Assert.AreEqual(0, model.Classes.SelectMany(c => c.Methods).Count(m => m.Verb == "Options"));
        }

        [Test]
        public async Task ShouldIncludePatchFromMethods()
        {
            var model = await GetCongoGeneratedModel();
            Assert.IsTrue(model.Classes.SelectMany(c => c.Methods).Count(m => m.Verb == "Patch") > 0);
        }

        [Test]
        public async Task ShouldParseArrays()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual(CollectionTypeHelper.GetCollectionType("Sales"), model.Objects.First(o => o.Name == "GetSales").Properties[1].Type);
        }

        [Test]
        public async Task ShouldParseCommonObjects()
        {
            var model = await GetTestGeneratedModel();
            Assert.AreEqual("Exchange", model.SchemaObjects["exchange"].Name);
            Assert.AreEqual(3, model.SchemaObjects["exchange"].Properties.Count);
        }

        [Test]
        public async Task ShouldParse_Congo()
        {
            var model = await GetCongoGeneratedModel();
            Assert.AreEqual(11, model.Classes.SelectMany(c => c.Methods).Count());
            Assert.AreEqual(13, model.Objects.Count());
        }

        [Test]
        public async Task ShouldBuildClasses_FromDars()
        {
            var model = await GetDarsGeneratedModel();
            Assert.AreEqual(2, model.SchemaObjects.Count());
        }

        [Test]
        public async Task ShouldBuildClasses_FromDarsWithParams()
        {
            var model = await GetDarsWithParamsGeneratedModel();
            Assert.AreEqual(2, model.SchemaObjects.Count());
        }


        [Test]
        public async Task ShouldBuildClasses_FromEpi()
        {
            var model = await GetEpiGeneratedModel();
            Assert.AreEqual(2, model.Classes.Count());
        }

        [Test]
        public async Task ShouldBuildClasses_FromFoo()
        {
            var model = await GetFooGeneratedModel();
            Assert.AreEqual(2, model.Classes.Count());
        }

        [Test]
        public async void Should_Name_Schemas_Using_Keys()
        {
            var model = await GetSchemaTestsGeneratedModel();
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Thing"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Things"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ThingResult"));
            Assert.IsTrue(model.Objects.Any(o => o.Name == "ThingRequest"));
        }

        [Test]
        public async void Should_Generate_Properties_When_Movies()
        {
            var model = await GetMoviesGeneratedModel();
            Assert.AreEqual(88, model.Objects.Sum(c => c.Properties.Count));
        }

        [Test]
        public async void Should_Generate_Properties_When_Contacts()
        {
            var model = await GetContactsGeneratedModel();
            Assert.AreEqual(5, model.Objects.Sum(c => c.Properties.Count));
        }

        [Test]
        public async void Should_Generate_Properties_When_Large()
        {
            var model = await GetLargeGeneratedModel();
            Assert.AreEqual(77, model.Objects.Sum(c => c.Properties.Count));
        }

        [Test]
        public async Task Should_Generate_Valid_XML_Comments_WhenMovies()
        {
            var model = await GetMoviesGeneratedModel();
            var xmlDoc = new XmlDocument();
            foreach (var method in model.Classes.SelectMany(c => c.Methods))
            {
                var xmlComment = GetXml(method.XmlComment);
                var xmlSimpleComment = GetXml(method.XmlSimpleComment);
                Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlComment));
                Assert.DoesNotThrow(() => xmlDoc.LoadXml(xmlSimpleComment));
            }
        }

        [Test]
        public async Task ShouldGenerateEnums_WhenFstab()
        {
            var model = await GetFstabGeneratedModel();
            Assert.AreEqual(4, model.Enums.Count());
            Assert.AreEqual(5, model.Objects.Sum(o => o.Properties.Count(p => p.IsEnum)));
        }

        [Test]
        public async Task ShouldGenerateModels_WhenExternalRefs()
        {
            var model = await GetExternalRefsGeneratedModel();
            Assert.AreEqual(4, model.Objects.Count());
        }

        [Test]
        public async Task ShouldGenerateDifferentModels_WhenDifferentPropertiesWithSameName()
        {
            var model = await GetSameNameGeneratedModel();
            Assert.AreEqual(2, model.Objects.Count(o => o.Name.ToLowerInvariant().Contains("director")));
        }

        [Test]
        public async Task ShouldGenerateProperties_Issue17()
        {
            var model = await GetIssue17GeneratedModel();
            Assert.IsTrue(model.SchemaObjects.Where(o => o.Key.EndsWith("Content")).All(o => o.Value.Properties.Count == 4));
        }

        [Test]
        public async Task ShouldNotDuplicateClasses()
        {
            var model = await GetDuplicationGeneratedModel();
            Assert.AreEqual(2, model.Objects.Count(o => o.Name.StartsWith("User")));
            Assert.AreEqual(1, model.Objects.Count(o => o.Name.Equals("User")));
        }

        [Test]
        public async Task ShouldGenerateMethodsForPatch()
        {
            var model = await GetPatchGeneratedModel();
            Assert.AreEqual(1, model.Classes.Sum(o => o.Methods.Count(m => m.Verb == "Patch")));
            Assert.AreEqual(2, model.Objects.Count(o => o.Name.Contains("Patch")));
        }

        [Test]
        public async Task ShouldGenerateRootResource()
        {
            var model = await GetRootGeneratedModel();
            Assert.IsTrue(model.Classes.Any(c => c.Name == "RootUrl"));
            Assert.AreEqual(3, model.Classes.First(c => c.Name == "RootUrl").Methods.Count);
        }

        [Test]
        public async Task ShouldAcceptWithoutBaseUri()
        {
            var model = await BuildModel("files/raml1/baseUri.raml");
            Assert.IsEmpty(model.BaseUri);
        }

        [Test] // JSON.NET issue
        public async Task ShouldHandleJsonSchemaRecursiveTree()
        {
            var raml = await new AmfParser().Load("files/tree-issue63.raml");
            var model = new ClientGeneratorService(raml, "test", "TargetNamespace", "TargetNamespace.Models").BuildModel();

            Assert.IsNotNull(model);
            Assert.AreEqual(1, model.Objects.Count());
        }

        private static string GetXml(string comment)
        {
            if (string.IsNullOrWhiteSpace(comment))
                return comment;

            return "<root>" + comment.Replace("///", string.Empty).Replace("\\\"", "\"") + "</root>";
        }

        private async Task<ClientGeneratorModel> GetTestGeneratedModel()
        {
            return await BuildModel("files/test.raml");
        }

        private async Task<ClientGeneratorModel> GetBoxGeneratedModel()
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

        private async Task<ClientGeneratorModel> GetMoviesGeneratedModel()
        {
            return await BuildModel("files/movies.raml");
        }

        private async Task<ClientGeneratorModel> GetFstabGeneratedModel()
        {
            return await BuildModel("files/fstab.raml");
        }

        private async Task<ClientGeneratorModel> GetDarsGeneratedModel()
        {
            return await BuildModel("files/dars.raml");
        }

        private async Task<ClientGeneratorModel> GetDarsWithParamsGeneratedModel()
        {
            return await BuildModel("files/darsparam.raml");
        }

        private async Task<ClientGeneratorModel> GetEpiGeneratedModel()
        {
            return await BuildModel("files/epi.raml");
        }

        private async Task<ClientGeneratorModel> GetFooGeneratedModel()
        {
            return await BuildModel("files/foo.raml");
        }

        private async Task<ClientGeneratorModel> GetSchemaTestsGeneratedModel()
        {
            return await BuildModel("files/schematests.raml");
        }


        private async Task<ClientGeneratorModel> GetExternalRefsGeneratedModel()
        {
            return await BuildModel("files/external-refs.raml");
        }

        private async Task<ClientGeneratorModel> GetSameNameGeneratedModel()
        {
            return await BuildModel("files/same-name-dif-obj.raml");
        }

        private async Task<ClientGeneratorModel> GetIssue17GeneratedModel()
        {
            return await BuildModel("files/issue17.raml");
        }

        private async Task<ClientGeneratorModel> GetDuplicationGeneratedModel()
        {
            return await BuildModel("files/duplication.raml");
        }

        private async Task<ClientGeneratorModel> GetPatchGeneratedModel()
        {
            return await BuildModel("files/patch.raml");
        }

        private async Task<ClientGeneratorModel> BuildModel(string ramlFile)
        {
            IncrementTestCount();
            var fi = new FileInfo(ramlFile);
            var raml = await new AmfParser().Load(fi.FullName);
            var model = new ClientGeneratorService(raml, "test", "TargetNamespace", "TargetNamespace.Models").BuildModel();

            return model;
        }

        private async Task<ClientGeneratorModel> GetRootGeneratedModel()
        {
            return await BuildModel("files/root.raml");
        }

    }
}