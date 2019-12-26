using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using Raml.Parser.Expressions;

namespace RAML.WebApiExplorer
{
    public class RamlController : Controller
    {
        public ActionResult Index()
        {
            ViewData["url"] = $"{Request.Url.Scheme}://{Request.Url.Authority}{Url.Content("~")}";
            return View();
        }

        public ActionResult Raw(string version = "1")
        {
            var ramlContents = GetRamlContents(version);
            return File(Encoding.UTF8.GetBytes(ramlContents), "text/raml");
        }

        public ActionResult Download(string version = "1")
        {
            var ramlContents = GetRamlContents(version);
            return File(Encoding.UTF8.GetBytes(ramlContents), "text/raml", "generated.raml");
        }

        private static string GetRamlContents(string version)
        {
            var config = GlobalConfiguration.Configuration;
            var apiExplorer = config.Services.GetApiExplorer();
            var ramlVersion = (version == "0.8" ? RamlVersion.Version08 : RamlVersion.Version1);
            ApiExplorerService apiExplorerService;
            if (ramlVersion == RamlVersion.Version1)
                apiExplorerService = new ApiExplorerServiceVersion1(apiExplorer, config.VirtualPathRoot);
            else
                apiExplorerService = new ApiExplorerServiceVersion08(apiExplorer, config.VirtualPathRoot);

            var ramlDocument = apiExplorerService.GetRaml(ramlVersion);
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