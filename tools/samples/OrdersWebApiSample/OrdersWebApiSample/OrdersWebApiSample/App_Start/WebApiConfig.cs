using System.Web.Http;

namespace OrdersWebApiSample
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            config.Formatters.Remove(config.Formatters.XmlFormatter);
            config.Formatters.Add(new RAML.Api.Core.XmlSerializerFormatter());

            // Web API routes
            config.MapHttpAttributeRoutes();
        }
    }
}
