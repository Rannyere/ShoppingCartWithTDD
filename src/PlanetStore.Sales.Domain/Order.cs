using System;
using System.Collections.Generic;
using System.Linq;
using PlanetStore.Core.DomainObjects;

namespace PlanetStore.Sales.Domain
{
    public class Order
    {
        public static int MAX_UNITS_ITEM => 15;
        public static int MIN_UNITS_ITEM => 1;

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
            if (orderItem.Quantity > MAX_UNITS_ITEM) throw new DomainException();

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
