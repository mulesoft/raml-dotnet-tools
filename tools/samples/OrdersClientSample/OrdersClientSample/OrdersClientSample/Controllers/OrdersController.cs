using System.Collections.Generic;
using System.Linq;
using System.Net;
using OrdersClientSample.App;
using OrdersClientSample.Models;
using OrdersClientSample.OrdersXml;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace OrdersClientSample.Controllers
{
    [RoutePrefix("orders")]
    public class OrdersController : Controller
    {
        private readonly OrdersXmlClient client;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            var fakeResponseHandler = new FakeResponseHandler(ordersRepository);
            var httpClient = new HttpClient(fakeResponseHandler) {BaseAddress = new Uri("http://test.com/api/")};
            client = new OrdersXmlClient(httpClient);
        }

        [Route("")]
        public async Task<ActionResult> Index()
        {
            ViewBag.Title = "Not Shipped Orders";
            var resp = await client.Orders.Notshipped.Get();
            var orders = resp.Content;
            var viewModels = Mappers.Map(orders);
            return View(viewModels);
        }

        [Route("{id}")]
        public async Task<ActionResult> Index(string id)
        {
            var resp = await client.Orders.Get(id);
            var order = resp.Content;
            var viewModel = Mappers.Map(order);
            return View("View", viewModel);
        }

        [Route("shipped")]
        public async Task<ActionResult> Shipped()
        {
            ViewBag.Title = "Shipped Orders";
            var resp = await client.Orders.Shipped.Get();
            var orders = resp.Content;
            var viewModels = Mappers.Map(orders);
            return View("Index", viewModels);
        }

        [Route("add")]
        public ActionResult Add()
        {
            ViewBag.Title = "Add Order";
            Products.Clear();
            return View("Edit", new OrderEditModel());
        }

        [Route("")]
        [HttpPost]
        public async Task<ActionResult> Save(OrderEditModel model)
        {
            model.Products = Products;
            var order = Mappers.Map(model);
            
            if(!string.IsNullOrWhiteSpace(order.id))
                await client.Orders.Put(order, order.id);
            else
                await client.Orders.Post(order);

            Products.Clear();
            return RedirectToAction("Index");
        }

        [Route("{id}/edit")]
        public async Task<ActionResult> Edit(string id)
        {
            ViewBag.Title = "Edit Order";
            var response = await client.Orders.Get(id);
            if(response.StatusCode == HttpStatusCode.NotFound)
                return new HttpNotFoundResult("Order not found " + id);

            Products = Mappers.Map(response.Content.items.item).ToList();

            return View("Edit", Mappers.MapToEditModel(response.Content));
        }

        [Route("addproduct")]
        [HttpPost]
        public ActionResult AddProduct(ProductViewModel model)
        {
            Products.Add(model);
            return Json(Products);
        }

        private IList<ProductViewModel> Products
        {
            get
            {
                if (Session["products"] == null)
                    Session["products"] = new List<ProductViewModel>();

                return (IList<ProductViewModel>)Session["products"];
            }

            set
            {
                Session["products"] = value;
            }
        }
    }
}