using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Expressions;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class MultipleTypesTest
    {

        private const string movieSchema = "         { \r\n"
                                     + "           \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n"
                                     + "           \"type\": \"object\",\r\n"
                                     + "           \"description\": \"a movie\",\r\n"
                                     + "           \"properties\": {\r\n"
                                     + "           \"id\": { \"type\": \"integer\"},\r\n"
                                     + "           \"name\": { \"type\": \"string\"},\r\n"
                                     + "           \"director\": { \"type\": \"string\"},\r\n"
                                     + "           \"genre\": { \"type\": \"string\" },\r\n"
                                     + "           \"cast\":{ \"type\": \"string\" },\r\n"
                                     + "           \"duration\":{ \"type\": \"number\" },\r\n"
                                     + "           \"storyline\":{ \"type\": \"string\" },\r\n"
                                     + "           \"language\":{ \"type\": \"string\" }\r\n"
                                     + "           }\r\n"
                                     + "         }\r\n";

        private const string errorSchema = "         { \r\n"
                                     + "           \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n"
                                     + "           \"type\": \"object\",\r\n"
                                     + "           \"description\": \"error\",\r\n"
                                     + "           \"properties\": {\r\n"
                                     + "           \"  code\": { \"type\": \"integer\"},\r\n"
                                     + "           \"  description\": { \"type\": \"string\"}\r\n"
                                     + "           }\r\n"
                                     + "         }\r\n";

        private const string notFoundSchema = "         { \r\n"
                                     + "           \"$schema\": \"http://json-schema.org/draft-03/schema\",\r\n"
                                     + "           \"type\": \"object\",\r\n"
                                     + "           \"description\": \"not found\",\r\n"
                                     + "           \"properties\": {\r\n"
                                     + "           \"  id\": { \"type\": \"integer\"},\r\n"
                                     + "           \"  code\": { \"type\": \"integer\"},\r\n"
                                     + "           \"  description\": { \"type\": \"string\"}\r\n"
                                     + "           }\r\n"
                                     + "         }\r\n";

        [TestMethod]
        public void Should_Get_Multiple_Type_When_Many_Response_Schemas_On_Root()
        {
            var doc = new RamlDocument {Title = "test"};
            var schemas = new List<Dictionary<string, string>>();
            var schemaMovie = new Dictionary<string, string>();
            schemaMovie.Add("movie", movieSchema);
            var schemaError = new Dictionary<string, string>();
            schemaMovie.Add("error", errorSchema);
            var schemaNotFound = new Dictionary<string, string>();
            schemaMovie.Add("notfound", notFoundSchema);
            schemas.Add(schemaMovie);
            schemas.Add(schemaError);
            schemas.Add(schemaNotFound);
            doc.Schemas = schemas;

            var okResponse = new Response
                             {
                                 Code = "200",
                                 Body = new Dictionary<string, MimeType>
                                        {
                                            {
                                                "application/json", new MimeType
                                                                    {
                                                                        Schema = "movie"
                                                                    }
                                            }
                                        }
                             };

            var errorResponse = new Response
                                {
                                    Code = "400",
                                    Body = new Dictionary<string, MimeType>
                                           {
                                               {
                                                   "application/json", new MimeType
                                                                       {
                                                                           Schema = "error"
                                                                       }
                                               }
                                           }
                                };

            var notFoundResponse = new Response
                                   {
                                       Code = "404",
                                       Body = new Dictionary<string, MimeType>
                                              {
                                                  {
                                                      "application/json", new MimeType
                                                                          {
                                                                              Schema = "notfound"
                                                                          }
                                                  }
                                              }
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
                                    RelativeUri = "/movies",
                                    Methods = methods
                                }
                            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TestNs");
			var model = service.BuildModel();
			Assert.AreEqual(1, model.ResponseObjects.Count);
            Assert.AreEqual(3, model.SchemaObjects.Count);

            var multipleModel = model.ResponseObjects.First(o => o.Key.Contains("Multiple")).Value;

            Assert.AreEqual(3, multipleModel.Properties.Count);

            Assert.AreEqual(1, multipleModel.Properties.Count(p => p.Type == "Movie"));
            Assert.AreEqual(1, multipleModel.Properties.Count(p => p.Type == "Error"));
            Assert.AreEqual(1, multipleModel.Properties.Count(p => p.Type == "Notfound"));

        }

        [TestMethod]
        public void Should_Get_Multiple_Type_When_Many_Response_Schemas_On_Resource()
        {
            var doc = new RamlDocument { Title = "test" };

            var okResponse = new Response
            {
                Code = "200",
                Body = new Dictionary<string, MimeType>
                                        {
                                            {
                                                "application/json", new MimeType
                                                                    {
                                                                        Schema = movieSchema
                                                                    }
                                            }
                                        }
            };

            var errorResponse = new Response
            {
                Code = "400",
                Body = new Dictionary<string, MimeType>
                                           {
                                               {
                                                   "application/json", new MimeType
                                                                       {
                                                                           Schema = errorSchema
                                                                       }
                                               }
                                           }
            };

            var notFoundResponse = new Response
            {
                Code = "404",
                Body = new Dictionary<string, MimeType>
                                              {
                                                  {
                                                      "application/json", new MimeType
                                                                          {
                                                                              Schema = notFoundSchema
                                                                          }
                                                  }
                                              }
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
                                    RelativeUri = "/movies",
                                    Methods = methods
                                }
                            };

            doc.Resources = resources;

            var service = new ClientGeneratorService(doc, "test", "TestNs");
			var model = service.BuildModel();
			Assert.AreEqual(4, model.ResponseObjects.Count);

            var multipleModel = model.ResponseObjects.First(o => o.Key.Contains("Multiple")).Value;

            Assert.AreEqual(3, multipleModel.Properties.Count);
        }
    }
}