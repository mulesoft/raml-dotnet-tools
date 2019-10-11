using Microsoft.Practices.Unity;
using System.Web.Http;
using OrdersWebApiSample.Repositories;
using Unity.WebApi;

namespace OrdersWebApiSample
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();
            
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            container.RegisterType<IOrdersRepository, OrdersRepository>();
            
            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}