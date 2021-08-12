using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanetStore.Sales.Domain
{
    public class Order
    {
        public decimal TotalValue { get; set; }

        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        public Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public void AddItem(OrderItem orderItem)
        {
            _orderItems.Add(orderItem);
            TotalValue = OrderItems.Sum(i => i.Quantity * i.UnitValue);
        }
    }
}
