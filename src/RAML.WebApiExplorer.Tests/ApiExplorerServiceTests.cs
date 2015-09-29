using System.Collections.ObjectModel;
using System.Reflection.Emit;
using System.Web.Http;
using System.Web.Http.Controllers;
using System.Web.Http.Description;
using System.Web.Http.Routing;
using Moq;
using NUnit.Framework;

namespace RAML.WebApiExplorer.Tests
{
	[TestFixture]
	public class ApiExplorerServiceTests
	{
		[Test]
		public void Test()
		{
			//var apiExplorer = new Mock<IApiExplorer>();
			//var apiDescriptions = new Collection<ApiDescription>();
			//var httpConfiguration = new HttpConfiguration();
			//var actionDescriptor =
			//	new ReflectedHttpActionDescriptor(
			//		new HttpControllerDescriptor(httpConfiguration, typeof (TestController).Name, typeof (TestController)),
			//		new TestController().GetType().GetMethod("Get"));
			//var apiDescriptionMock = new Mock<ApiDescription>();
			//apiDescriptionMock.Setup(x => x.ResponseDescription).Returns(new ResponseDescription());
			//apiDescriptionMock.Setup(x => x.ActionDescriptor).Returns(actionDescriptor);
			//apiDescriptionMock.Setup(x => x.RelativePath).Returns("test");
			//apiDescriptionMock.Setup(x => x.Route).Returns(new HttpRoute("test/{id}"));
			//apiDescriptions.Add(apiDescriptionMock.Object);
			//apiExplorer.Setup(x => x.ApiDescriptions).Returns(apiDescriptions);

			var routes = new HttpRouteCollection();
			routes.Add("test", new HttpRoute("test/{id}"));
			var conf = new HttpConfiguration(routes);
			var apiExplorer = conf.Services.GetApiExplorer();
			var apiExplorerService = new ApiExplorerService(apiExplorer, "http://test.com");
			var ramlDoc = apiExplorerService.GetRaml();
			Assert.IsNotNull(ramlDoc);
		}

	}
}