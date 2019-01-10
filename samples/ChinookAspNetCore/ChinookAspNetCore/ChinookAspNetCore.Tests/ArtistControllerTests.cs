using System;
using ChinookAspNetCore.ChinookV1;
using ChinookAspNetCore.ChinookV1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ChinookAspNetCore.Tests
{
    [TestClass]
    public class ArtistControllerTests
    {
        protected ArtistControllerTestsImplementation implementation = new ArtistControllerTestsImplementation();
        
        [TestMethod]
        public void GetByIdTest()
        {
            // Arrange
            IArtistsController controller = new ArtistsController();
            controller = implementation.GetByIdArrange(controller);

            // Act
            var result = controller.GetById("1");

            // Assert
            Assert.IsInstanceOfType(result, typeof(IActionResult));
            var objectResult = result as OkObjectResult;
            Assert.IsNotNull(objectResult);
            Assert.IsInstanceOfType(objectResult.Value, typeof(Artist));
            implementation.GetByIdAssert(result);
        }
    }
}
