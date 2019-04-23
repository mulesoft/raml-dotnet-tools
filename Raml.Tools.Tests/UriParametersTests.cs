using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class UriParametersTests
    {
        [TestMethod]
        public void should_build_uri_parameter_objects()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParams = new Dictionary<string, Parameter>()
                            {
                                {
                                    "code", new Parameter
                                            {
                                                DisplayName = "code",
                                                Type = "integer"
                                            }
                                },
                                {
                                    "token", new Parameter
                                             {
                                                 DisplayName = "token",
                                                 Type = "string"
                                             }
                                }
                            };



            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get",
                              },
                              new Method
                              {
                                  Verb = "post"
                              }
                          };

            var resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/movies",
                                    UriParameters = uriParams,
                                    Methods = methods
                                }
                            };

            doc.Resources = resources;


			var service = new ClientGeneratorService(doc, "test", "NamespaceTest");
			var model = service.BuildModel();

            Assert.AreEqual(1, model.UriParameterObjects.Count);
            Assert.AreEqual(2, model.UriParameterObjects.First().Value.Properties.Count);
            Assert.IsTrue(model.Classes.First().Methods.All(m => m.UriParametersType == "MoviesUriParameters"));
            Assert.AreEqual(1, model.Objects.Count());
        }

        [TestMethod]
        public void should_keep_original_names()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParams = new Dictionary<string, Parameter>()
                            {
                                {
                                    "code-oneOne", new Parameter
                                            {
                                                DisplayName = "code",
                                                Type = "integer"
                                            }
                                },
                                {
                                    "token_Two", new Parameter
                                             {
                                                 DisplayName = "token",
                                                 Type = "string"
                                             }
                                }
                            };



            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get",
                              },
                              new Method
                              {
                                  Verb = "post"
                              }
                          };

            var resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/movies",
                                    UriParameters = uriParams,
                                    Methods = methods
                                }
                            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TestNs");
            var model = service.BuildModel();

            Assert.AreEqual("code-oneOne", model.UriParameterObjects.First().Value.Properties.First(p => p.Name == "CodeoneOne").OriginalName);
            Assert.AreEqual("token_Two", model.UriParameterObjects.First().Value.Properties.First(p => p.Name == "Token_Two").OriginalName);
        }
    }
}