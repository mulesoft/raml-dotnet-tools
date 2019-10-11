using System.Collections.Generic;
using System.Linq;
using OrdersClientSample.OrdersXml.Models;

namespace OrdersClientSample.App
{
    public interface IOrdersRepository
    {
        void Add(PurchaseOrderType order);
        void Update(PurchaseOrderType order);
        PurchaseOrderType Get(string id);
        PurchaseOrdersType GetShipped();
        PurchaseOrdersType GetNotShipped();
    }

    public class OrdersRepository : IOrdersRepository
    {
        private static readonly IDictionary<string, PurchaseOrderType> orders = new Dictionary<string, PurchaseOrderType>();

        public void Add(PurchaseOrderType order)
        {
            var id = orders.Count + 1;
            order.id = id.ToString();
            orders.Add(order.id, order);
        }

        public void Update(PurchaseOrderType order)
        {
            orders[order.id] = order;
        }

        public PurchaseOrderType Get(string id)
        {
            return orders[id];
        }

        public PurchaseOrdersType GetShipped()
        {
            var collection = new PurchaseOrderType[orders.Values.Count(o => o.shipped)];
            var shippedOrders = orders.Values.Where(o => o.shipped);
            var i = 0;
            foreach (var order in shippedOrders)
            {
                collection[i] = order;
                i++;
            }
            
            return new PurchaseOrdersType { orders = collection };
        }

        public PurchaseOrdersType GetNotShipped()
        {
            var collection = new PurchaseOrderType[orders.Values.Count(o => !o.shipped)];
            var notShippedOrders = orders.Values.Where(o => !o.shipped);
            var i = 0;
            foreach (var order in notShippedOrders)
            {
                collection[i] = order;
                i++;
            }

            return new PurchaseOrdersType { orders = collection };
        }
    }
}