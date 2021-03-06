﻿using System.IO;
using NUnit.Framework;
using RAML.Parser;
using AMF.Tools.Core.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class Oas30Tests
    {
        public int TestCount = 0;
        private void IncrementTestCount()
        {
            TestCount++;
        }

        [Test]
        public async Task PetStoreServer()
        {
            var model = await BuildWebApiModel("files/oas/3.0/petstore.yaml");
            Assert.AreEqual(1, model.Controllers.Count());
        }

        [Test]
        public async Task PetStoreClient()
        {
            var model = await BuildClientModel("files/oas/3.0/petstore.yaml");
            Assert.AreEqual(2, model.Classes.Count());
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Pet"));
        }

        [Test]
        public async Task PetStoreExpandedClient()
        {
            var model = await BuildClientModel("files/oas/3.0/petstore-expanded.yaml");
            Assert.AreEqual(2, model.Classes.Count());
            Assert.IsTrue(model.Objects.Any(o => o.Name == "Pet"));
        }

        private async Task<WebApiGeneratorModel> BuildWebApiModel(string ramlFile)
        {
            IncrementTestCount();
            var fi = new FileInfo(ramlFile);
            var raml = await new RamlParser().Load(fi.FullName);
            return new WebApiGeneratorService(raml, "TestNs", "TestNs.Models").BuildModel();
        }

        private async Task<ClientGeneratorModel> BuildClientModel(string ramlFile)
        {
            IncrementTestCount();
            var fi = new FileInfo(ramlFile);
            var raml = await new RamlParser().Load(fi.FullName);
            return new ClientGeneratorService(raml, "root", "TestNs", "TestsNs.Models").BuildModel();
        }

    }
}