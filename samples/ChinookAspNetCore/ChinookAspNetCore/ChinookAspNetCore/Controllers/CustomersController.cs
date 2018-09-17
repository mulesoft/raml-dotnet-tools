// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChinookAspNetCore.ChinookV1.Models;

namespace ChinookAspNetCore.ChinookV1
{
    public partial class CustomersController : ICustomersController
    {

/// <summary>
		/// /customers
		/// </summary>
		/// <returns>IList&lt;Customer&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: customers/customers
			// var result = new IList<Customer>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /customers
		/// </summary>
		/// <param name="customer"></param>
        public IActionResult Post([FromBody] Models.Customer customer)
        {
            // TODO: implement Post - route: customers/customers
			return new ObjectResult("");
        }

/// <summary>
		/// /customers/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Customer</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: customers/{id}
			// var result = new Customer();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /customers/{id}
		/// </summary>
		/// <param name="customer"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Customer customer,string id)
        {
            // TODO: implement Put - route: customers/{id}
			return new ObjectResult("");
        }

/// <summary>
		/// /customers/{id}
		/// </summary>
		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: customers/{id}
			return new ObjectResult("");
        }

    }
}
