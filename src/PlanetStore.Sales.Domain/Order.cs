using System;
using System.Collections.Generic;
using System.Linq;

namespace PlanetStore.Sales.Domain
{
    public class Order
    {
        public Guid CustomerId { get; private set; }
        public decimal TotalValue { get; private set; }
        public OrderStatus OrderStatus { get; private set; }
        
        private readonly List<OrderItem> _orderItems;
        public IReadOnlyCollection<OrderItem> OrderItems => _orderItems;

        protected Order()
        {
            _orderItems = new List<OrderItem>();
        }

        public void CalculateTotalValueOrder()
        {
            TotalValue = OrderItems.Sum(i => i.CalculateValue());
        }

        public void AddItem(OrderItem orderItem)
        {
            if (_orderItems.Any(p => p.ProductId == orderItem.ProductId))
            {
                var itemExisting = _orderItems.FirstOrDefault(p => p.ProductId == orderItem.ProductId);
                itemExisting.AddUnits(orderItem.Quantity);
                orderItem = itemExisting;

                _orderItems.Remove(itemExisting);
            }

            _orderItems.Add(orderItem);
            CalculateTotalValueOrder();
        }

        public void CreateDraft()
        {
            OrderStatus = OrderStatus.draft;
        }

        public static class OrderFactory
        {
            public static Order NewOrderDraft(Guid customerId)
            {
                var order = new Order
                {
                    CustomerId = customerId,
                };

                order.CreateDraft();
                return order;
            }
        }
    }
}
