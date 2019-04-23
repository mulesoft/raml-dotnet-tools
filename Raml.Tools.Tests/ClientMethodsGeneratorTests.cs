using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class ClientMethodsGeneratorTests
    {
        [TestMethod]
        public void Should_Generate_One_Method_Per_Verb()
        {
            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              },
                              new Method
                              {
                                  Verb = "post"
                              }
                          };

            var resource = new Resource
            {
                RelativeUri = "/abc{token}{code}",
                Methods = methods,
            };

            var schemaResponseObjects = new Dictionary<string, ApiObject>();
            var schemaRequestObjects = new Dictionary<string, ApiObject>();
            var ramlDocument = new RamlDocument();
            var uriParameterObjects = new Dictionary<string, ApiObject>();
            var queryObjects = new Dictionary<string, ApiObject>();
            var headerObjects = new Dictionary<string, ApiObject>();
            var responseHeadersObjects = new Dictionary<string, ApiObject>();
            var linkedKeyWithObjectNames = new Dictionary<string, string>();
            var classObject = new ClassObject();
            var schemaObjects = new Dictionary<string, ApiObject>();

            var generator = new ClientMethodsGenerator(ramlDocument, schemaResponseObjects,
                uriParameterObjects, queryObjects, headerObjects, responseHeadersObjects, schemaRequestObjects, linkedKeyWithObjectNames,
                schemaObjects);

            var generatorMethods = generator.GetMethods(resource, "/", classObject, "Test", new Dictionary<string, Parameter>());

            Assert.AreEqual(2, generatorMethods.Count);
        }

        [TestMethod]
        public void Should_parse_parameters_in_descriptions()
        {
            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get",
                                  Description = "resourcePathName: <<resourcePathName>>, resourcePath: <<resourcePath>>, methodName: <<methodName>>"
                              },
                              new Method
                              {
                                  Verb = "post",
                                  Description = "resourcePathName: <<resourcePathName>>, resourcePath: <<resourcePath>>, methodName: <<methodName>>"
                              }
                          };

            var resource = new Resource
            {
                RelativeUri = "/abc{token}{code}",
                Methods = methods,
            };

            var schemaResponseObjects = new Dictionary<string, ApiObject>();
            var schemaRequestObjects = new Dictionary<string, ApiObject>();
            var ramlDocument = new RamlDocument();
            var uriParameterObjects = new Dictionary<string, ApiObject>();
            var queryObjects = new Dictionary<string, ApiObject>();
            var headerObjects = new Dictionary<string, ApiObject>();
            var responseHeadersObjects = new Dictionary<string, ApiObject>();
            var linkedKeyWithObjectNames = new Dictionary<string, string>();
            var classObject = new ClassObject();
            var schemaObjects = new Dictionary<string, ApiObject>();

            var generator = new ClientMethodsGenerator(ramlDocument, schemaResponseObjects,
                uriParameterObjects, queryObjects, headerObjects, responseHeadersObjects, schemaRequestObjects, linkedKeyWithObjectNames,
                schemaObjects);

            var generatorMethods = generator.GetMethods(resource, "http://www.sample.com/abc", classObject, "Test", new Dictionary<string, Parameter>());

            Assert.AreEqual("resourcePathName: abc, resourcePath: /abc, methodName: get", generatorMethods.First().Comment);
            Assert.AreEqual("resourcePathName: abc, resourcePath: /abc, methodName: post", generatorMethods.Last().Comment);
        }
    }
}