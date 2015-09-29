using System.Web.Http;

namespace RAML.WebApiExplorer.Tests
{
	[RoutePrefix("test")]
	public class TestController : ApiController
	{
		[Route("{id}")]
		public IHttpActionResult Get(int id)
		{
			return Ok();
		}
	}
}