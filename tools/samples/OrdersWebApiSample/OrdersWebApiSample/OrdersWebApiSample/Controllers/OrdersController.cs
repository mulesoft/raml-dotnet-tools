// Template: Controller Implementation (ApiControllerImplementation.t4) version 3.0

using OrdersWebApiSample.Repositories;
using System.Web.Http;

namespace OrdersWebApiSample.OrdersXml
{
    public partial class OrdersController : IOrdersController
    {
        private readonly IOrdersRepository ordersRepository;

        public OrdersController(IOrdersRepository ordersRepository)
        {
            this.ordersRepository = ordersRepository;
        }

        /// <summary>
		/// Create a new purchase order
		/// </summary>
		/// <param name="purchaseordertype"></param>
        public IHttpActionResult Post(Models.PurchaseOrderType purchaseordertype)
        {
            ordersRepository.Add(purchaseordertype);
			return Ok();
        }

		/// <summary>
		/// gets already shipped orders
		/// </summary>
		/// <returns>PurchaseOrdersType</returns>
        public IHttpActionResult Get()
		{
		    var result = ordersRepository.GetShipped();
            return Ok(result);
        }

		/// <summary>
		/// gets not shipped orders
		/// </summary>
		/// <returns>PurchaseOrdersType</returns>
        public IHttpActionResult GetNotshipped()
        {
            var result = ordersRepository.GetNotShipped(); 
            return Ok(result);
        }

		/// <summary>
		/// gets an order by id
		/// </summary>
		/// <param name="id"></param>
		/// <returns>PurchaseOrderType</returns>
        public IHttpActionResult GetById([FromUri] string id)
        {
			return Ok(ordersRepository.Get(id));
        }

		/// <summary>
		/// updates an order
		/// </summary>
		/// <param name="purchaseordertype"></param>
		/// <param name="id"></param>
        public IHttpActionResult Put(Models.PurchaseOrderType purchaseordertype,[FromUri] string id)
        {
            ordersRepository.Update(purchaseordertype);
			return Ok();
        }

		/// <summary>
		/// marks order as shipped
		/// </summary>
		/// <param name="content"></param>
		/// <param name="id"></param>
        public IHttpActionResult PostShip([FromBody] string content,[FromUri] string id)
		{
		    var order = ordersRepository.Get(id);
		    order.shipped = true;
            ordersRepository.Update(order);
			return Ok();
        }

    }
}
