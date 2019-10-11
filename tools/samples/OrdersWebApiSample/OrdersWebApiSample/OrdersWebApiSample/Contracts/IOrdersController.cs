// Template: Controller Interface (ApiControllerInterface.t4) version 3.0

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using OrdersWebApiSample.OrdersXml.Models;


namespace OrdersWebApiSample.OrdersXml
{
    public interface IOrdersController
    {

        IHttpActionResult Post(Models.PurchaseOrderType purchaseordertype);
        IHttpActionResult Get();
        IHttpActionResult GetNotshipped();
        IHttpActionResult GetById([FromUri] string id);
        IHttpActionResult Put(Models.PurchaseOrderType purchaseordertype,[FromUri] string id);
        IHttpActionResult PostShip([FromBody] string content,[FromUri] string id);
    }
}
