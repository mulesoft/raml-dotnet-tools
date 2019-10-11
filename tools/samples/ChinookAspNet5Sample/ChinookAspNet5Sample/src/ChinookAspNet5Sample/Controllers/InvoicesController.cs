// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using ChinookAspNet5Sample.ChinookV1.Models;

namespace ChinookAspNet5Sample.ChinookV1
{
    public partial class InvoicesController : IInvoicesController
    {

		/// <returns>IList&lt;Invoice&gt;</returns>
        public IActionResult Get()
        {
            // TODO: implement Get - route: invoices/
			// var result = new IList<Invoice>();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
        public IActionResult Post([FromBody] Models.Invoice content)
        {
            // TODO: implement Post - route: invoices/
			return new ObjectResult("");
        }

		/// <param name="id"></param>
		/// <returns>Invoice</returns>
        public IActionResult GetById(string id)
        {
            // TODO: implement GetById - route: invoices/{id}
			// var result = new Invoice();
			// return new ObjectResult(result);
			return new ObjectResult("");
        }

		/// <param name="content"></param>
		/// <param name="id"></param>
        public IActionResult Put([FromBody] Models.Invoice content,string id)
        {
            // TODO: implement Put - route: invoices/{id}
			return new ObjectResult("");
        }

		/// <param name="id"></param>
        public IActionResult Delete(string id)
        {
            // TODO: implement Delete - route: invoices/{id}
			return new ObjectResult("");
        }

    }
}
