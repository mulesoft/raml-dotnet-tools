using Raml.Parser.Expressions;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using System.Web.Mvc;
using System.Linq;
using RAML.WebApiExplorer;
using System.Threading.Tasks;

namespace OAuthWebApi.Controllers
{
	public class RamlController : Controller
	{
		public ActionResult Index()
		{
            return File("MyRaml.raml", "application/raml");
		}
        			
	}
}