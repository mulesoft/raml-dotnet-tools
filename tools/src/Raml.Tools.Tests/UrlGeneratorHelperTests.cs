using Microsoft.VisualStudio.TestTools.UnitTesting;
using Raml.Tools.WebApiGenerator;

namespace Raml.Tools.Tests
{
    [TestClass]
    public class UrlGeneratorHelperTests
    {
        [TestMethod]
        public void should_get_relative_uri()
        {
            Assert.AreEqual("users", UrlGeneratorHelper.GetRelativeUri("movies/{id}/users", "/movies/{id}"));
        }

        [TestMethod]
        public void should_replace_consecutive_params()
        {
            Assert.AreEqual("{mediaTypeExtension}/users", UrlGeneratorHelper.GetRelativeUri("movies/{id}{mediaTypeExtension}/users", "/movies/{id}"));
        }

        [TestMethod]
        public void should_replace_duplicated_params()
        {
            Assert.AreEqual("users{mediaTypeExtension}", UrlGeneratorHelper.GetRelativeUri("movies/{id}/{mediaTypeExtension}/users{mediaTypeExtension}", "/movies/{id}"));
        }

        [TestMethod]
        public void should_avoid_replacing_route_contained_in_uri()
        {
            Assert.AreEqual("{applicationId}", UrlGeneratorHelper.GetRelativeUri("/application/{applicationId}", "application"));
        }

        [TestMethod]
        public void should_fix_controller_uri()
        {
            Assert.AreEqual("movies/{id}", UrlGeneratorHelper.FixControllerRoutePrefix("/movies/{id}{mediaTypeExtension}"));
        }

        [TestMethod]
        public void should_fix_controller_uri2()
        {
            Assert.AreEqual("movies", UrlGeneratorHelper.FixControllerRoutePrefix("/movies/{mediaTypeExtension}"));
        }
    }
}