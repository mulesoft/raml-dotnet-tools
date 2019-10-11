using Microsoft.AspNetCore.Mvc;

namespace RAML.NetCoreApiExplorer.Tests
{
	[Route("test")]
	public class TestController : Controller
	{
		[Route("{id}")]
		public IActionResult Get(int id)
		{
			return Ok();
		}
	}
}