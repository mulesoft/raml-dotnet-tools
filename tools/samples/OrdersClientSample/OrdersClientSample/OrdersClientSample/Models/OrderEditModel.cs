using System;
using System.Collections.Generic;
using OrdersClientSample.OrdersXml.Models;

namespace OrdersClientSample.Models
{
    public class OrderEditModel
    {
        public OrderEditModel()
        {
            Date = DateTime.Now;
            ShipTo = new AddressType();
            BillTo = new AddressType();
            Products = new List<ProductViewModel>();
        }

        public string Id { get; set; }
        public DateTime Date { get; set; }
        public bool Shipped { get; set; }
        public AddressType ShipTo { get; set; }
        public AddressType BillTo { get; set; }
        public IList<ProductViewModel> Products { get; set; }
    }
}