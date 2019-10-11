using System;
using System.Collections.Generic;

namespace OrdersClientSample.Models
{
    public class OrderViewModel
    {
        public string Id { get; set; }
        public DateTime Date { get; set; }
        public bool Shipped { get; set; }
        public IEnumerable<string> Addresses { get; set; }
        public IEnumerable<ProductViewModel> Products { get; set; }
    }
}