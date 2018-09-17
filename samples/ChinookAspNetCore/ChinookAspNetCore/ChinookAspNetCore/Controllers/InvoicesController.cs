// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ChinookAspNetCore.ChinookV1.Models;

namespace ChinookAspNetCore.ChinookV1
{
    public partial class InvoicesController : IInvoicesController
    {

/// <summary>
		/// /invoices
		/// </summary>
		/// <returns>IList&lt;Invoice&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: invoices/invoices
			// var result = new IList<Invoice>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /invoices
		/// </summary>
		/// <param name="invoice"></param>
        public IActionResult Post([FromBody] Models.Invoice invoice)
        {
            // TODO: implement Post - route: invoices/invoices
			return new ObjectResult("");
        }

/// <summary>
		/// /invoices/{id}
		/// </summary>
		/// <param name="id"></param>
		/// <returns>Invoice</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: invoices/{id}
			// var result = new Invoice();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

/// <summary>
		/// /invoices/{id}
		/// </summary>
		/// <param name="invoice"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Invoice invoice,string id)
        {
            // TODO: implement Put - route: invoices/{id}
			return new ObjectResult("");
        }

/// <summary>
		/// /invoices/{id}
		/// </summary>
		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: invoices/{id}
			return new ObjectResult("");
        }

    }
}
