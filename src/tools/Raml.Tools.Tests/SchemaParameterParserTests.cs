using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser;
using Raml.Parser.Expressions;
using Raml.Tools.Pluralization;
using System.Collections.Generic;
using System.Linq;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class SchemaParameterParserTests
    {
        private Resource searchResource;
        private Resource deliveriesResource;
        private Resource foldersResource;
        private readonly SchemaParameterParser schemaParameterParser = new SchemaParameterParser(new EnglishPluralizationService());

        [TestInitialize]
        public void Setup()
        {
            var raml = new RamlParser().Load("files/box.raml");
            var raml2 = new RamlParser().Load("files/congo-drones-5-f.raml");
            deliveriesResource = raml2.Resources.First(r => r.RelativeUri == "/deliveries");
            searchResource = raml.Resources.First(r => r.RelativeUri == "/search");
            foldersResource = raml.Resources.First(r => r.RelativeUri == "/folders");
        }


        [TestMethod]
        public void ShouldPluralize()
        {
            var actual = schemaParameterParser.Parse("<<resourcePathName | !pluralize>>", searchResource, searchResource.Methods.First(), searchResource.RelativeUri);
            Assert.AreEqual("searches", actual);
        }

        [TestMethod]
        public void ShouldPluralize_WhenNoSpace()
        {
            var actual = schemaParameterParser.Parse("<<resourcePathName|!pluralize>>", searchResource, searchResource.Methods.First(), searchResource.RelativeUri);
            Assert.AreEqual("searches", actual);
        }

        [TestMethod]
        public void ShouldSingularize_WhenIrregular()
        {
            var actual = schemaParameterParser.Parse("<<resourcePathName | !singularize>>", deliveriesResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("delivery", actual);
        }

        [TestMethod]
        public void ShouldSingularize_WhenRegular()
        {
            var actual = schemaParameterParser.Parse("<<resourcePathName | !singularize>>", foldersResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("folder", actual);
        }

        [TestMethod]
        public void ShouldSingularize_WhenNoSpace()
        {
            var actual = schemaParameterParser.Parse("<<resourcePathName|!singularize>>", deliveriesResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("delivery", actual);
        }

        [TestMethod]
        public void ShouldGetResourcePathName()
        {
            var actual = schemaParameterParser.Parse("<<resourcePathName>>", deliveriesResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("deliveries", actual);
        }

        [TestMethod]
        public void ShouldGetResourcePath()
        {
            var actual = schemaParameterParser.Parse("<<resourcePath>>", deliveriesResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("/deliveries", actual);
        }

        [TestMethod]
        public void ShouldGetMethod()
        {
            var actual = schemaParameterParser.Parse("<<methodName>>", deliveriesResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("get", actual);
        }

        [TestMethod]
        public void ShouldGetMixedParameter()
        {
            var actual = schemaParameterParser.Parse("<<methodName>>new<<resourcePathName | !singularize>>", deliveriesResource, deliveriesResource.Methods.First(), deliveriesResource.RelativeUri);
            Assert.AreEqual("getnewdelivery", actual);
        }

        [TestMethod]
        public void ShouldGetParameter_WhenChildResource()
        {
            var resource = new Resource
                           {
                               RelativeUri = "/{songId}",
                           };
            var actual = schemaParameterParser.Parse("<<resourcePathName | !singularize>>", resource, new Method(), "/songs");
            Assert.AreEqual("song", actual);
        }

        [TestMethod]
        public void ShouldGetParameter_WhenChildResourceAddSlash()
        {
            var resource = new Resource
            {
                RelativeUri = "/{songId}",
            };
            var actual = schemaParameterParser.Parse("<<resourcePathName | !singularize>>", resource, new Method(), "songs");
            Assert.AreEqual("song", actual);
        }

        [TestMethod]
        public void ShoudParseCustomParameters()
        {
            var type = new Dictionary<string, IDictionary<string, string>>();
            var typeParams = new Dictionary<string, string> {{"customParam", "valueOfParam"}};
            type.Add("type", typeParams);
            var resource = new Resource
            {
                RelativeUri = "/test",
                Type = type
            };

            var actual = schemaParameterParser.Parse("<<customParam>>", resource, new Method(), "/test");
            Assert.AreEqual("valueOfParam", actual);
        }
    }
}