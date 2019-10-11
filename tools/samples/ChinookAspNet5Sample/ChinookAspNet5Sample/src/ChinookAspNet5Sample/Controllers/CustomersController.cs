// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;

namespace ChinookAspNet5Sample.ChinookV1
{
    public partial class CustomersController : ICustomersController
    {

		/// <returns>IList&lt;Customer&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: customers/
			// var result = new IList<Customer>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
        public IActionResult Post([FromBody] Models.Customer content)
        {
            // TODO: implement Post - route: customers/
			return new ObjectResult("");
        }

		/// <param name="id"></param>
		/// <returns>Customer</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: customers/{id}
			// var result = new Customer();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Customer content,string id)
        {
            // TODO: implement Put - route: customers/{id}
			return new ObjectResult("");
        }

		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: customers/{id}
			return new ObjectResult("");
        }

    }
}
