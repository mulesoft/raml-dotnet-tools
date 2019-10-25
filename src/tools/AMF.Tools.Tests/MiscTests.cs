using System.IO;
using NUnit.Framework;
using AMF.Parser;
using AMF.Tools.Core.WebApiGenerator;
using System.Linq;
using System.Threading.Tasks;
using AMF.Tools.Core.Pluralization;
using System;

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

        private string operationOnWord(string arg)
        {
            return arg;
        }
    }
}