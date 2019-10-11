using System.Web.Routing;

namespace AuthorizationCodeGrant
{
    public class Global : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}