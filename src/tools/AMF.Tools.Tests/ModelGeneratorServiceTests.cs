using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class ModelGeneratorServiceTests
    {
        public int TestCount = 0;
        private void IncrementTestCount()
        {
            TestCount++;
        }

        [Test]
        public async Task PetStoreModels()
        {
            IncrementTestCount();
            var fi = new FileInfo("files/oas/petstore.json");
            var raml = await new AmfParser().Load(fi.FullName);
            var model = new ModelsGeneratorService(raml, "test").BuildModel();
            Assert.AreEqual(3, model.Objects.Count());
        }

        [Test]
        public async Task MoviesModels()
        {
            IncrementTestCount();
            var fi = new FileInfo("files/raml1/movies-v1.raml");
            var raml = await new AmfParser().Load(fi.FullName);
            var model = new ModelsGeneratorService(raml, "test").BuildModel();
            Assert.AreEqual(1, model.Objects.Count());
        }

    }
}