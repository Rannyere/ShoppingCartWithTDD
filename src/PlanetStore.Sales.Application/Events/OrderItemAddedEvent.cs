using System;
using MediatR;
using PlanetStore.Core;

namespace PlanetStore.Sales.Application.Events
{
    public class OrderItemAddedEvent : Event
    {
        public Guid CustomerId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid ProductId { get; private set; }
        public string ProductName { get; set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        public OrderItemAddedEvent(Guid customerId, Guid orderId, Guid productId, string productName, int quantity, decimal unitValue)
        {
            CustomerId = customerId;
            OrderId = orderId;
            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }
    }
}
