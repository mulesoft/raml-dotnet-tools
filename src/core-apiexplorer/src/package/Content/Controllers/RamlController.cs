using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace RAML.WebApiExplorer
{
    [Route("raml")]
    public class RamlController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("raw")]
        public IActionResult Raw(string version = null)
        {
            return Ok();
        }
    }
}
