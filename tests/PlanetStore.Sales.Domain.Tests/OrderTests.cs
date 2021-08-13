using System;
using System.Linq;
using Xunit;

namespace PlanetStore.Sales.Domain.Tests
{
    public class OrderTests
    {
        [Fact(DisplayName = "Add Item to Order empty")]
        [Trait("Category", "Order Tests")]
        public void AddItemOrder_NewOrder_MustUpdateValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(200, order.TotalValue);
        }

        [Fact(DisplayName = "Add Item to Order existing")]
        [Trait("Category", "Order Tests")]
        public void AddItemOrder_ItemExisting_MustIncrementUnitsUpdateValues()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 2, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Product Test", 1, 100);

            // Act
            order.AddItem(orderItem2);

            // Assert
            Assert.Equal(300, order.TotalValue);
            Assert.Equal(1, order.OrderItems.Count);
            Assert.Equal(3, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
        }
    }
}
