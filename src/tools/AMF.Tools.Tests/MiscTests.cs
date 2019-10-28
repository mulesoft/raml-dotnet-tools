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