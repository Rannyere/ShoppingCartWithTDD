using System;
using PlanetStore.Core.DomainObjects;

namespace PlanetStore.Sales.Domain
{
    public class OrderItem
    {
        public Guid ProductId { get; private set; }
        public string ProductName { get; private set; }
        public int Quantity { get; private set; }
        public decimal UnitValue { get; private set; }

        public OrderItem(Guid productId, string productName, int quantity, decimal unitValue)
        {
            if (quantity < Order.MIN_UNITS_ITEM) throw new DomainException();

            ProductId = productId;
            ProductName = productName;
            Quantity = quantity;
            UnitValue = unitValue;
        }

        public void AddUnits(int units)
        {
            Quantity += units;
        }

        public decimal CalculateValue()
        {
            return Quantity * UnitValue;
        }
    }
}
