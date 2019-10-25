using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.Pluralization;
using System;
using AMF.Tools.Core;

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
                Verb = "Get"
            };
            Assert.AreEqual("string", cm.OkReturnType);
            Assert.AreEqual("string", cm.OkReturnType);
        }

        [Test]
        public void ReamlTypesHelperTest()
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
            Assert.AreEqual("type", result);

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