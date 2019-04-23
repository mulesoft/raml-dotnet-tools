using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;
using Raml.Tools.WebApiGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class UriParametersGeneratorTests
    {

        [TestMethod]
        public void Should_Build_Uri_Parameters()
        {
            var uriParameters = new Dictionary<string, Parameter>
                                {
                                    {
                                        "code", new Parameter
                                                {
                                                    DisplayName = "code",
                                                    Type = "integer"
                                                }
                                    }
                                };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              }
                          };

            var resource = new Resource
                                {
                                    RelativeUri = "/abc{token}{code}",
                                    Methods = methods,
                                    UriParameters = uriParameters
                                };

            var generator = new UriParametersGenerator(new Dictionary<string, ApiObject>());
            var uriParameterObjects = new Dictionary<string, ApiObject>();
            var generatorMethod = new ClientGeneratorMethod { Name = "MethodOne"};

            generator.Generate(resource, "/movies/{bucket}/abc{token}{code}", generatorMethod, uriParameterObjects, new Dictionary<string, Parameter>());

            Assert.AreEqual(1, uriParameterObjects.Count);
            Assert.AreEqual(3, generatorMethod.UriParameters.Count());
            Assert.AreEqual(uriParameterObjects.First().Value.Name, generatorMethod.UriParametersType);
        }

        [TestMethod]
        public void Should_Build_Uri_Parameters_From_Resources()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParameters = new Dictionary<string, Parameter>
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
                                  Verb = "get"
                              }
                          };

            var resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/movies/{token}/abc/{code}/asd/{extra}",
                                    Methods = methods,
                                    UriParameters = uriParameters
                                },
                                new Resource
                                {
                                    RelativeUri = "/directors/{a}/abc/{b}{c}/asd/{d}",
                                    Methods = methods
                                }
                            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TestNs");
			var model = service.BuildModel();
			Assert.AreEqual(2, model.UriParameterObjects.Count);

            var moviesUriParamObj = model.UriParameterObjects.First(o => o.Key.ToLowerInvariant().Contains("movies")).Value;
            var directorsUriParamObj = model.UriParameterObjects.First(o => o.Key.ToLowerInvariant().Contains("directors")).Value;

            Assert.AreEqual(3, moviesUriParamObj.Properties.Count);
            Assert.AreEqual(4, directorsUriParamObj.Properties.Count);
        }

        [TestMethod]
        public void Should_Build_Uri_Parameters_When_Nested_Resources()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParameters = new Dictionary<string, Parameter>
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
                                  Verb = "get"
                              }
                          };

            var resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/movies/{token}{extra}/abc/{code}{extra}/asd",
                                    Methods = methods,
                                    UriParameters = uriParameters,
                                    Resources = new[]
                                                {
                                                    new Resource
                                                    {
                                                        RelativeUri = "/{a}{b}",
                                                        Methods = methods
                                                    }

                                                }
                                }
                            };

            doc.Resources = resources;


            var service = new ClientGeneratorService(doc, "test", "TestNs");
			var model = service.BuildModel();
			Assert.AreEqual(2, model.UriParameterObjects.Count);

            var moviesUriParamObj = model.UriParameterObjects.First().Value;
            var directorsUriParamObj = model.UriParameterObjects.Last().Value;

            Assert.AreEqual(3, moviesUriParamObj.Properties.Count);
            Assert.AreEqual(5, directorsUriParamObj.Properties.Count);
        }


        [TestMethod]
        public void Should_Inherit_Uri_Parameters_When_Nested_Resources()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParameters1 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "id", new Parameter
                                                {
                                                    DisplayName = "id",
                                                    Type = "integer"
                                                }
                                    }
                                };

            var uriParameters2 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "current", new Parameter
                                                 {
                                                     DisplayName = "current",
                                                     Type = "boolean"
                                                 }
                                    }
                                };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              }
                          };

            var resources = new Collection<Resource>
            {
                new Resource
                {
                    RelativeUri = "/movies",
                    Methods = methods,
                    Resources = new[]
                    {
                        new Resource
                        {
                            RelativeUri = "/{id}",
                            Methods = methods,
                            UriParameters = uriParameters1,
                            Resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/something",
                                    Methods = methods,
                                    Resources = new Collection<Resource>
                                    {
                                        new Resource
                                        {
                                            RelativeUri = "/{current}",
                                            Methods = methods,
                                            UriParameters = uriParameters2,
                                            Resources = new Collection<Resource>
                                            {
                                                new Resource
                                                {
                                                    RelativeUri = "/deep",
                                                    Methods = methods
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            doc.Resources = resources;

            var service = new WebApiGeneratorService(doc, "TargetNamespace");
            var model = service.BuildModel();
            Assert.AreEqual("bool", model.Controllers.First().Methods.First(m => m.Name.Contains("Deep")).UriParameters.First(p => p.Name == "current").Type);
            Assert.AreEqual("int", model.Controllers.First().Methods.First(m => m.Name.Contains("Deep")).UriParameters.First(p => p.Name == "id").Type);
        }

        [TestMethod]
        public void Should_Inherit_Uri_Parameters_When_Nested_Resources_Case2()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParameters1 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "id", new Parameter
                                                {
                                                    DisplayName = "id",
                                                    Type = "integer"
                                                }
                                    }
                                };

            var uriParameters2 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "current", new Parameter
                                                 {
                                                     DisplayName = "current",
                                                     Type = "boolean"
                                                 }
                                    }
                                };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              }
                          };

            var resources = new Collection<Resource>
            {
                new Resource
                {
                    RelativeUri = "/{id}",
                    Methods = methods,
                    UriParameters = uriParameters1,
                    Resources = new Collection<Resource>
                    {
                        new Resource
                        {
                            RelativeUri = "/something",
                            Methods = methods,
                            Resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/{current}",
                                    Methods = methods,
                                    UriParameters = uriParameters2,
                                    Resources = new Collection<Resource>
                                    {
                                        new Resource
                                        {
                                            RelativeUri = "/deep",
                                            Methods = methods
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            };

            doc.Resources = resources;

            var service = new WebApiGeneratorService(doc, "TargetNamespace");
            var model = service.BuildModel();
            Assert.AreEqual("bool", model.Controllers.First().Methods.First(m => m.Name.Contains("Deep")).UriParameters.First(p => p.Name == "current").Type);
            Assert.AreEqual("int", model.Controllers.First().Methods.First(m => m.Name.Contains("Deep")).UriParameters.First(p => p.Name == "id").Type);
        }

        [TestMethod]
        public void Should_Inherit_Uri_Parameters_When_Nested_Resources_Client()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParameters1 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "id", new Parameter
                                                {
                                                    DisplayName = "id",
                                                    Type = "integer"
                                                }
                                    }
                                };

            var uriParameters2 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "current", new Parameter
                                                 {
                                                     DisplayName = "current",
                                                     Type = "boolean"
                                                 }
                                    }
                                };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              }
                          };

            var resources = new Collection<Resource>
            {
                new Resource
                {
                    RelativeUri = "/movies",
                    Methods = methods,
                    Resources = new[]
                    {
                        new Resource
                        {
                            RelativeUri = "/{id}",
                            Methods = methods,
                            UriParameters = uriParameters1,
                            Resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/something",
                                    Methods = methods,
                                    Resources = new Collection<Resource>
                                    {
                                        new Resource
                                        {
                                            RelativeUri = "/{current}",
                                            Methods = methods,
                                            UriParameters = uriParameters2,
                                            Resources = new Collection<Resource>
                                            {
                                                new Resource
                                                {
                                                    RelativeUri = "/deep",
                                                    Methods = methods
                                                }
                                            }

                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TargetNamespace");
            var model = service.BuildModel();
            Assert.AreEqual("bool", model.Classes.First(m => m.Name.Contains("Deep")).Methods.First().UriParameters.First(p => p.Name == "current").Type);
            Assert.AreEqual("int", model.Classes.First(m => m.Name.Contains("Deep")).Methods.First().UriParameters.First(p => p.Name == "id").Type);
        }

        [TestMethod]
        public void Should_Inherit_Uri_Parameters_When_Nested_Resources_ClientCase2()
        {
            var doc = new RamlDocument { Title = "test" };

            var uriParameters1 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "id", new Parameter
                                                {
                                                    DisplayName = "id",
                                                    Type = "integer"
                                                }
                                    }
                                };

            var uriParameters2 = new Dictionary<string, Parameter>
                                {
                                    {
                                        "current", new Parameter
                                                 {
                                                     DisplayName = "current",
                                                     Type = "boolean"
                                                 }
                                    }
                                };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              }
                          };

            var resources = new Collection<Resource>
            {
                new Resource
                {
                    RelativeUri = "/{id}",
                    Methods = methods,
                    UriParameters = uriParameters1,
                    Resources = new Collection<Resource>
                    {
                        new Resource
                        {
                            RelativeUri = "/something",
                            Methods = methods,
                            Resources = new Collection<Resource>
                            {
                                new Resource
                                {
                                    RelativeUri = "/{current}",
                                    Methods = methods,
                                    UriParameters = uriParameters2,
                                    Resources = new Collection<Resource>
                                    {
                                        new Resource
                                        {
                                            RelativeUri = "/deep",
                                            Methods = methods
                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TargetNamespace");
            var model = service.BuildModel();
            Assert.AreEqual("bool", model.Classes.First(m => m.Name.Contains("Deep")).Methods.First().UriParameters.First(p => p.Name == "current").Type);
            Assert.AreEqual("int", model.Classes.First(m => m.Name.Contains("Deep")).Methods.First().UriParameters.First(p => p.Name == "id").Type);
        }

        [TestMethod]
        public void Should_Build_Uri_Parameters_When_Custom_Scalars()
        {
            var uriParameters = new Dictionary<string, Parameter>
                                {
                                    {
                                        "id", new Parameter
                                                {
                                                    DisplayName = "id",
                                                    Type = "CustomInt"
                                                }
                                    }
                                };

            var methods = new List<Method>
                          {
                              new Method
                              {
                                  Verb = "get"
                              }
                          };

            var resource = new Resource
            {
                RelativeUri = "/abc{id}",
                Methods = methods,
                UriParameters = uriParameters
            };

            var schemaObjects = new Dictionary<string, ApiObject>();
            schemaObjects.Add("CustomInt", new ApiObject
            {
                Type = "CustomInt",
                Name = "CustomInt",
                Properties = new[] { new Property { Name = "value", Type = "int"} }
            });
            var generator = new UriParametersGenerator(schemaObjects);
            var uriParameterObjects = new Dictionary<string, ApiObject>();
            var generatorMethod = new ClientGeneratorMethod { Name = "MethodOne" };

            generator.Generate(resource, "/movies/abc{id}", generatorMethod, uriParameterObjects, new Dictionary<string, Parameter>());

            Assert.AreEqual("CustomInt", generatorMethod.UriParameters.First().Type);
        }
    }
}