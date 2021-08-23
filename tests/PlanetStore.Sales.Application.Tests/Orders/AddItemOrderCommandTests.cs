using System;
using System.Linq;
using PlanetStore.Sales.Application.Commands;
using PlanetStore.Sales.Domain;
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

        [Fact(DisplayName = "Add Item Command Valid")]
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

        [Fact(DisplayName = "Add Item Command Invalid")]
        [Trait("Category", "Order Commands Tests")]
        public void AddItemOrderCommand_CommandIsInvalid_MustBeInvalid()
        {
            // Arrange
            var orderCommand = new AddItemOrderCommand(Guid.Empty, Guid.Empty, "", 0, 0);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddItemOrderCommandValidation.CustomerIdErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddItemOrderCommandValidation.ProductIdErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddItemOrderCommandValidation.ProductNameErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddItemOrderCommandValidation.QuantityMinErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
            Assert.Contains(AddItemOrderCommandValidation.UnitValueErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }

        [Fact(DisplayName = "Add Item Command with Units above allowed")]
        [Trait("Category", "Order Commands Tests")]
        public void AdddItemOrderCommand_QuantityUnitsAboveAllowed_MustNotPassTheValidation()
        {
            // Arrange
            var orderCommand = new AddItemOrderCommand(_customerId, _productId, "Product Test", Order.MAX_UNITS_ITEM + 1, 100);

            // Act
            var result = orderCommand.IsValid();

            // Assert
            Assert.False(result);
            Assert.Contains(AddItemOrderCommandValidation.QuantityMaxErrorMsg, orderCommand.ValidationResult.Errors.Select(c => c.ErrorMessage));
        }
    }
}
