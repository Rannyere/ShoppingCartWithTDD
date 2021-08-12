using System;
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
            var order = new Order();
            var orderItem = new OrderItem(Guid.NewGuid(), "Product Test", 2, 100);

            // Act
            order.AddItem(orderItem);

            // Assert
            Assert.Equal(200, order.TotalValue);
        }
    }
}
