using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using OrdersWebApiSample.OrdersXml.Models;
using OrdersWebApiSample.Repositories;

namespace OrdersWebApiSample
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            UnityConfig.RegisterComponents();

            var ordersRepository = new OrdersRepository();

            var order = new PurchaseOrderType();
            order.shipped = false;
            order.orderDateSpecified = true;
            order.orderDate = DateTime.Now;
            order.items = new ItemsType();

            var items = new Collection<ItemsTypeItem>();
            items.Add(new ItemsTypeItem
            {
                productName = "XBOX One",
                partNum = "5465653",
                quantity = "1",
                USPrice = 400
            });
            order.items.item = items.ToArray();

            var addresses = new Collection<AddressType>();
            addresses.Add(new AddressType
            {
                city = "LA",
                street = "35, Rodeo Dr"
            });
            order.Items = addresses.ToArray();
            order.ItemsElementName = new[] { ItemsChoiceType.shipTo };
            ordersRepository.Add(order);

            var order2 = new PurchaseOrderType();
            order2.shipped = true;
            order2.orderDateSpecified = true;
            order2.orderDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            order2.items = new ItemsType();
            var items2 = new Collection<ItemsTypeItem>();
            items2.Add(new ItemsTypeItem
            {
                productName = "iPod Nano",
                partNum = "32312312",
                quantity = "2",
                USPrice = 200
            });
            order2.items.item = items2.ToArray();
            var addresses2 = new Collection<AddressType>();
            addresses2.Add(new AddressType
            {
                city = "NY",
                street = "433, Fith Av."
            });
            order2.ItemsElementName = new[] { ItemsChoiceType.shipTo };
            order2.Items = addresses2.ToArray();

            ordersRepository.Add(order2);

            var order3 = new PurchaseOrderType();
            order3.shipped = true;
            order3.orderDateSpecified = true;
            order3.orderDate = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0));
            order3.items = new ItemsType();

            var items3 = new Collection<ItemsTypeItem>();
            items3.Add(new ItemsTypeItem
            {
                productName = "iPod Nano Touch",
                partNum = "32312888",
                quantity = "1",
                USPrice = 250,
                shipDate = DateTime.Now,
                weightKg = 1
            });
            order3.items.item = items3.ToArray();

            ordersRepository.Add(order3);
        }
    }
}
