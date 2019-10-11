using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using OrdersClientSample.Models;
using OrdersClientSample.OrdersXml;
using OrdersClientSample.OrdersXml.Models;

namespace OrdersClientSample.Controllers
{
    public class HomeController : Controller
    {
        public async Task<ActionResult> Index()
        {
            return View();
        }
    }
}