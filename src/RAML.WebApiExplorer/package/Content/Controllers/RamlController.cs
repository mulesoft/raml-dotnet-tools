using System.Text;
using System.Web.Http;
using System.Web.Mvc;


namespace RAML.WebApiExplorer
{
	public class RamlController : Controller
	{
		public ActionResult Index()
		{
			return View();
		}

		public ActionResult Raw()
		{
            var ramlContents = GetRamlContents();
			return File(Encoding.UTF8.GetBytes(ramlContents), "text/raml");
		}

        public ActionResult Download()
        {
            var ramlContents = GetRamlContents();
            return File(Encoding.UTF8.GetBytes(ramlContents), "text/raml", "generated.raml");
        }

	    private static string GetRamlContents()
	    {
	        var config = GlobalConfiguration.Configuration;
	        var apiExplorer = config.Services.GetApiExplorer();
	        var apiExplorerService = new ApiExplorerService(apiExplorer, config.VirtualPathRoot);
	        var ramlDocument = apiExplorerService.GetRaml();
	        var ramlContents = new RamlSerializer().Serialize(ramlDocument);
	        return ramlContents;
	    }

	    public ActionResult OAuth1()
		{
			return View();
		}

		public ActionResult OAuth2()
		{
			return View();
		}		
	}
}