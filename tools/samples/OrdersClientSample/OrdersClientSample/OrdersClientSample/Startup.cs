using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(OrdersClientSample.Startup))]
namespace OrdersClientSample
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            
        }
    }
}
