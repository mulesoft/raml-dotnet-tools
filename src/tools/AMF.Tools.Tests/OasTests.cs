using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class OasTests
    {
        [Test]
        public async Task PetStoreServer()
        {
            var model = await BuildWebApiModel("files/oas/petstore.json");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task PetStoreClient()
        {
            var model = await BuildClientodel("files/oas/petstore.json");
            Assert.AreEqual(2, model.Classes.Count());
        }

        private static async Task<WebApiGeneratorModel> BuildWebApiModel(string ramlFile)
        {
            var fi = new FileInfo(ramlFile);
            var raml = await new AmfParser().Load(fi.FullName);
            return new WebApiGeneratorService(raml, "TestNs").BuildModel();
        }

        private static async Task<ClientGeneratorModel> BuildClientodel(string ramlFile)
        {
            var fi = new FileInfo(ramlFile);
            var raml = await new AmfParser().Load(fi.FullName);
            return new ClientGeneratorService(raml, "root", "TestNs").BuildModel();
        }

    }
}