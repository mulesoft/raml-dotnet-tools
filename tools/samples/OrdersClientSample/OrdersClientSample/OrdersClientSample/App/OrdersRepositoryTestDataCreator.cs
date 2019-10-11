using System;
using System.Collections.ObjectModel;
using System.Linq;
using OrdersClientSample.OrdersXml.Models;

namespace OrdersClientSample.App
{
    public class OrdersRepositoryTestDataCreator
    {
        public static void InitializeRepository()
        {
            var ordersRepository = new OrdersRepository();

            var order = new PurchaseOrderType();
            order.shipped = false;
            order.orderDateSpecified = true;
            order.orderDate = DateTime.Now;
            order.items = new ItemsType();

            var items = new Collection<ItemsTypeItem>();
            items.Add(new ItemsTypeItem
            {
                productName = "XBOX One",
                partNum = "5465653",
                quantity = "1",
                USPrice = 400
            });
            order.items.item = items.ToArray();

            var addresses = new Collection<AddressType>();
            addresses.Add(new AddressType
            {
                name = "John Doe",
                city = "LA",
                street = "35, Rodeo Dr"
            });
            order.Items = addresses.ToArray();
            order.ItemsElementName = new[] { ItemsChoiceType.shipTo };
            ordersRepository.Add(order);

            var order2 = new PurchaseOrderType();
            order2.shipped = true;
            order2.orderDateSpecified = true;
            order2.orderDate = DateTime.Now.Subtract(new TimeSpan(1, 0, 0, 0));
            order2.items = new ItemsType();
            var items2 = new Collection<ItemsTypeItem>();
            items2.Add(new ItemsTypeItem
            {
                productName = "iPod Nano",
                partNum = "32312312",
                quantity = "2",
                USPrice = 200
            });
            order2.items.item = items2.ToArray();
            var addresses2 = new Collection<AddressType>();
            addresses2.Add(new AddressType
            {
                name = "Jack Smith",
                city = "NY",
                street = "433, Fith Av."
            });
            order2.ItemsElementName = new[] { ItemsChoiceType.shipTo };
            order2.Items = addresses2.ToArray();

            ordersRepository.Add(order2);

            var order3 = new PurchaseOrderType();
            order3.shipped = true;
            order3.orderDateSpecified = true;
            order3.orderDate = DateTime.Now.Subtract(new TimeSpan(5, 0, 0, 0));
            order3.items = new ItemsType();

            var items3 = new Collection<ItemsTypeItem>();
            items3.Add(new ItemsTypeItem
            {
                productName = "iPod Nano Touch",
                partNum = "32312888",
                quantity = "1",
                USPrice = 250,
                shipDate = DateTime.Now,
                weightKg = 1
            });
            order3.items.item = items3.ToArray();

            var addresses3 = new Collection<AddressType>();
            addresses3.Add(new AddressType
            {
                name = "Will Jackson",
                city = "NY",
                street = "433, Fith Av."
            });
            addresses3.Add(new AddressType
            {
                name = "Mary Jackson",
                city = "NY",
                street = "433, Fith Av."
            });
            order3.ItemsElementName = new[] { ItemsChoiceType.shipTo, ItemsChoiceType.billTo };
            order3.Items = addresses3.ToArray();
            
            ordersRepository.Add(order3);
        }
    }
}