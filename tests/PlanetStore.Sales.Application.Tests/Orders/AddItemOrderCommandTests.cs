using System;
using PlanetStore.Sales.Application.Commands;
using Xunit;

namespace PlanetStore.Sales.Application.Tests.Orders
{
    public class AddItemOrderCommandTests
    {
        private readonly Guid _customerId;
        private readonly Guid _productId;

        public AddItemOrderCommandTests()
        {
            _customerId = Guid.NewGuid();
            _productId = Guid.NewGuid();
        }

        [Fact(DisplayName = "Add Item Command Valid ")]
        [Trait("Category", "Order Commands Tests")]
        public void AddItemOrderCommand_CommandIsValid_MustBeValid()
        {
            // Arrange
            var orderCommand = new AddItemOrderCommand(_customerId, _productId, "Product Test", 2, 100);

            // Act
            var result = orderCommand.IsValid();

            //Assert
            Assert.True(result);
        }
    }
}
