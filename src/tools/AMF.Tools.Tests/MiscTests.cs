using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.Pluralization;
using System;
using AMF.Tools.Core;
using System.Collections.Generic;
using AMF.Parser.Model;
using AMF.Tools.Core.ClientGenerator;

namespace Raml.Tools.Tests
{
    [TestFixture]
    public class MiscTests
    {
        public int TestCount = 0;
        private void IncrementTestCount()
        {
            TestCount++;
        }

        [Test]
        public void PluralizationTest()
        {
            IncrementTestCount();
            var result = PluralizationServiceUtil.DoesWordContainSuffix("pepes", new string[] { "es" }, System.Globalization.CultureInfo.InvariantCulture);
            Assert.IsTrue(result);
        }

        [Test]
        public void PluralizationTest2()
        {
            IncrementTestCount();
            var result = PluralizationServiceUtil.TryInflectOnSuffixInWord("pep", new string[] { "es" }, operationOnWord,
                System.Globalization.CultureInfo.InvariantCulture, out string pepes);
            Assert.IsFalse(result);
        }

        [Test]
        public void XmlCommentHelperTest()
        {
            IncrementTestCount();
            var result = XmlCommentHelper.Escape("<&>");
            Assert.AreEqual("&lt;&amp;&gt;", result);
        }

        [Test]
        public void ClientGeneratorMethodTest()
        {
            IncrementTestCount();
            var method = new ClientGeneratorMethod()
            {
                Name = "pepe",
                ResponseType = "string",
                ReturnType = "string",
                OkReturnType = "string",
                Url = "/",
                RequestType = "string",
                RequestContentTypes = new string[] { "application/json" },
                Parameter = new GeneratorParameter() { Name = "string", Type = "string" }
            };
            Assert.AreEqual(0, method.UriParametersCount);
            Assert.AreEqual(false, method.RequestIsXml);
            Assert.IsNotNull(method.SimpleReturnTypeString);
            Assert.IsNotNull(method.NetHttpMethod);
            Assert.IsNotNull(method.ParameterString);
            Assert.IsNotNull(method.ParameterStringCore);
            Assert.IsNotNull(method.QualifiedParameterType);
            Assert.IsNotNull(method.SimpleParameterString);
            Assert.IsNotNull(method.XmlComment);
            Assert.IsNotNull(method.XmlSimpleComment);
        }

        [Test]
        public void ResponseTypeServiceTest()
        {
            IncrementTestCount();
            var schemaObjs = new Dictionary<string, ApiObject>() { { "1",
                    new ApiObject() {
                        AmfId = "1",
                        Name = "Pepe",
                        Type = "string"
                    } 
                }
            };
            var objs = new Dictionary<string, ApiObject>();
            var links = new Dictionary<string, string>();
            var enums = new Dictionary<string, ApiEnum>();

            var payload = new Payload("application/json", new Shape("1", "Pepe", "", "", null, null, null, null));
            var payloads = new List<Payload>() { payload };
            var responses = new List<Response>() { new Response("200", "", "200", null, payloads, null) };
            var service = new ResponseTypesService(schemaObjs, objs, links, enums);
            var method = new Operation("GET", "test", "", false, "", new Documentation("www.doc.com", "docs", "docs"), new string[] { "HTTP" }, null,
                                        new string[] { "application/json" }, null, responses, null);
            var operations = new List<Operation>() { method };
            
            var resource = new EndPoint("Pepe","","",operations, null, null);
            var mimeType = payload;
            var fullUrl = "/";
            var key = "Pepe";
            var responseCode = "200";
            
            var result = service.GetResponseType(method, resource, mimeType, key, responseCode, fullUrl);
            Assert.AreEqual("string", result);
        }

        [Test]
        public void BidirectionDicTest()
        {
            IncrementTestCount();
            var dic = new Dictionary<string, string>() { { "a", "1" }, { "b", "2" } };
            var result = new StringBidirectionalDictionary(dic);
            var revDic = result.SecondToFirstDictionary;
            result.AddValue("c", "3");
            Assert.AreEqual(true, result.ExistsInFirst("b"));
            Assert.AreEqual(false, result.ExistsInSecond("asdsdas"));
            Assert.AreEqual("a", result.GetFirstValue("1"));
            Assert.AreEqual("1", result.GetSecondValue("a"));
        }

        [Test]
        public void WebApiGeneratorModelTest()
        {
            IncrementTestCount();
            var model = new WebApiGeneratorModel()
            {
                ApiVersion = "v1",
                BaseUri = "www.base.com",
                BaseUriParameters = new List<GeneratorParameter>() { new GeneratorParameter() { Name = "pepe", Type = "string" } },
                Controllers = new List<ControllerObject>(),
                ControllersNamespace = "ns",
                Security = new Security()
            };
            Assert.AreEqual("string pepe", model.BaseUriParametersString);
        }

        [Test]
        public void ControllerMethodTest()
        {
            IncrementTestCount();
            var cm = new ControllerMethod()
            {
                ModelsNamespace = "Models",
                Name = "method",
                Parameter = new GeneratorParameter() { Name = "pepe", Type = "string", ParamName = "pepe" },
                ResponseStatusCode = "200",
                ReturnType = "string",
                Verb = "Post"
            };
            Assert.AreEqual("string", cm.OkReturnType);
            Assert.AreEqual("string", cm.OkConcreteType);
            Assert.AreEqual("pepe", cm.ParametersCallString);
            Assert.AreEqual("string pepe = default(string);", cm.ParametersDefinitionAspNetCore);
            Assert.AreEqual("[FromBody] string pepe", cm.ParametersString);
            Assert.AreEqual("[FromBody] string pepe", cm.ParametersStringForAspNet5);
        }

        [Test]
        public void RamlTypesHelperTest()
        {
            IncrementTestCount();
            var result = RamlTypesHelper.ExtractType("type[][]");
            Assert.AreEqual("type", result);
            result = RamlTypesHelper.ExtractType("type[]");
            Assert.AreEqual("type", result);
            result = RamlTypesHelper.ExtractType("type{}");
            Assert.AreEqual("type", result);

            var apiObj = new ApiObject()
            {
                IsArray = true,
                Type = "type[]"
            };
            result = RamlTypesHelper.GetTypeFromApiObject(apiObj);
            Assert.AreEqual("IList<type[]>", result);

            apiObj = new ApiObject()
            {
                IsMap = true,
                Type = "type"
            };
            result = RamlTypesHelper.GetTypeFromApiObject(apiObj);
            Assert.AreEqual("type", result);

            apiObj = new ApiObject()
            {
                Type = "type"
            };
            result = RamlTypesHelper.GetTypeFromApiObject(apiObj);
            Assert.AreEqual("type", result);

            apiObj = new ApiObject()
            {
                Name = "type"
            };
            result = RamlTypesHelper.GetTypeFromApiObject(apiObj);
            Assert.AreEqual("type", result);
        }

        private string operationOnWord(string arg)
        {
            return arg;
        }
    }
}