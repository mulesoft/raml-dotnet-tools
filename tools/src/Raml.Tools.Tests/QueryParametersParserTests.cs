using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Parser.Builders;
using Raml.Tools.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class QueryParametersParserTests
    {
        [TestMethod]
        public void should_parse_query_parameters()
        {
            var parameterDynamicRaml = new Dictionary<string, object>
                                       {
                                           {"type", "string"},
                                           {"displayName", "ParameterName"},
                                           {"description", "this is the description"}
                                       };

            var parameters = new Dictionary<string, object> {{"one", parameterDynamicRaml}};

            var dynamicRaml = new Dictionary<string, object> {{"method", "get"}, {"queryParameters", parameters}};

            var queryParametersParser = new QueryParametersParser(new Dictionary<string, ApiObject>());
            var parsedParameters = queryParametersParser.ParseParameters(new MethodBuilder().Build(dynamicRaml, "application/json"));
            Assert.AreEqual(1, parsedParameters.Count);
            Assert.AreEqual("string", parsedParameters.First().Type);
            Assert.AreEqual("One", parsedParameters.First().Name);
        }

        [TestMethod]
        public void should_parse_query_object()
        {
            var parameterDynamicRaml = new Dictionary<string, object>
                                       {
                                           {"type", "string"},
                                           {"displayName", "ParameterName"},
                                           {"description", "this is the description"}
                                       };

            var parameters = new Dictionary<string, object> {{"one", parameterDynamicRaml}};

            var dynamicRaml = new Dictionary<string, object> {{"method", "get"}, {"queryParameters", parameters}};

            var generatedMethod = new ClientGeneratorMethod { Name = "GeneratedMethod"};
            const string objectName = "ObjName";
            var queryParametersParser = new QueryParametersParser(new Dictionary<string, ApiObject>());
            var queryObj = queryParametersParser.GetQueryObject(generatedMethod, new MethodBuilder().Build(dynamicRaml, "application/json"), objectName);
            var parsedParameters = queryObj.Properties;

            Assert.AreEqual(generatedMethod.Name + objectName + "Query", queryObj.Name);
            Assert.AreEqual(1, parsedParameters.Count);
            Assert.AreEqual("string", parsedParameters.First().Type);
            Assert.AreEqual("One", parsedParameters.First().Name);
        }

        [TestMethod]
        public void should_keep_original_names()
        {
            var parameterDynamicRaml = new Dictionary<string, object>
                                       {
                                           {"type", "string"},
                                           {"displayName", "parameter-name"},
                                           {"description", "this is the description"}
                                       };

            var parameters = new Dictionary<string, object> { { "keep-orig-name", parameterDynamicRaml } };

            var dynamicRaml = new Dictionary<string, object> { { "method", "get" }, { "queryParameters", parameters } };
            var queryParametersParser = new QueryParametersParser(new Dictionary<string, ApiObject>());
            var parsedParameters = queryParametersParser.ParseParameters(new MethodBuilder().Build(dynamicRaml, "application/json"));
            Assert.AreEqual("keep-orig-name", parsedParameters.First(p => p.Name == "Keeporigname").OriginalName);
        }
    }
}