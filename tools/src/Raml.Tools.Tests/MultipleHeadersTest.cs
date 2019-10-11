using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class MultipleHeadersTest
    {


        [TestMethod]
        public void Should_Build_Multiple_Headers_When_Many_Response_With_Headers()
        {
            var doc = new RamlDocument {Title = "test"};

            var okHeaders = new Dictionary<string, Parameter>
            {
                {
                    "code",
                    new Parameter
                    {
                        DisplayName = "code",
                        Type = "integer"
                    }
                },
                {
                    "token",
                    new Parameter
                    {
                        DisplayName = "token",
                        Type = "string"
                    }
                }
            };

            var errorHeaders = new Dictionary<string, Parameter>
            {
                {
                    "code",
                    new Parameter
                    {
                        DisplayName = "code",
                        Type = "integer"
                    }
                },
                {
                    "error",
                    new Parameter
                    {
                        DisplayName = "error",
                        Type = "string"
                    }
                }
            };

            var badRequestHeaders = new Dictionary<string, Parameter>
            {
                {
                    "code",
                    new Parameter
                    {
                        DisplayName = "code",
                        Type = "integer"
                    }
                },
                {
                    "description",
                    new Parameter
                    {
                        DisplayName = "description",
                        Type = "string"
                    }
                }
            };

            var okResponse = new Response
                             {
                                 Code = "200",
                                 Headers = okHeaders
                             };

            var errorResponse = new Response
                                {
                                    Code = "400",
                                    Headers = errorHeaders
                                };

            var notFoundResponse = new Response
                                   {
                                       Code = "404",
                                       Headers = badRequestHeaders
                                   };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get",
                                  Responses = new[] { okResponse, errorResponse, notFoundResponse }
                              }
                          };

            var resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "movies",
                                    Methods = methods
                                }
                            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TargetNamespace");
            var model = service.BuildModel();
            Assert.AreEqual(4, model.ResponseHeaderObjects.Count);

            var multipleModel = model.ResponseHeaderObjects.First(o => o.Key.Contains("Multiple")).Value;

            Assert.AreEqual(3, multipleModel.Properties.Count);

            Assert.AreEqual("Models." + model.ResponseHeaderObjects.First(o => o.Key.Contains("Multiple")).Value.Name,
                model.Classes.First().Methods.First().ResponseHeaderType);
        }


    }
}