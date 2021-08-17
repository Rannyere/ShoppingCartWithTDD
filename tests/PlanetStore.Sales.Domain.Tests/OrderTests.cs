using System;
using System.Linq;
using PlanetStore.Core.DomainObjects;
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

        [Fact(DisplayName = "New Item with Units above allowed")]
        [Trait("Category", "Order Tests")]
        public void NewItemOrder_UnitsItemAboveAllowed_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", Order.MAX_UNITS_ITEM + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem));
        }

        [Fact(DisplayName = "Add Units ExistingItem with Units above allowed")]
        [Trait("Category", "Order Tests")]
        public void AddItemOrder_AddUnitsItemExistingAboveAllowed_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 1, 100);
            order.AddItem(orderItem);

            var orderItem2 = new OrderItem(productId, "Product Test", Order.MAX_UNITS_ITEM, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.AddItem(orderItem2));
        }

        [Fact(DisplayName = "Update Item Nonexistent")]
        [Trait("Category", "Order Tests")]
        public void UpdateItemOrder_UpdateItemNonexistent_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItemUpdate = new OrderItem(Guid.NewGuid(), "Product Test", 5, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(orderItemUpdate));

        }

        [Fact(DisplayName = "Update Units Valid Item")]
        [Trait("Category", "Order Tests")]
        public void UpdateItemOrder_UpdateUnitsValidItem_MustUpdateQuantity()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 1, 100);
            order.AddItem(orderItem);
            var orderItemUpdated = new OrderItem(productId, "Product Test", Order.MAX_UNITS_ITEM, 100);
            var newQuantity = orderItemUpdated.Quantity;

            // Act
            order.UpdateItem(orderItemUpdated);

            //Assert
            Assert.Equal(newQuantity, order.OrderItems.FirstOrDefault(p => p.ProductId == productId).Quantity);
        }

        [Fact(DisplayName = "Update Order Item Validade Total")]
        [Trait("Category", "Order Tests")]
        public void UpdateOrderItem_OrderwithDifferentProducts_MustUpdateTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItemExistent1 = new OrderItem(Guid.NewGuid(), "Product XYZ", 2, 100);
            var orderItemExistent2 = new OrderItem(productId, "Product Test", 3, 15);
            order.AddItem(orderItemExistent1);
            order.AddItem(orderItemExistent2);

            var orderItemUpdated = new OrderItem(productId, "Product Teste", 5, 15);

            var totalPedido = orderItemExistent1.Quantity * orderItemExistent1.UnitValue +
                              orderItemUpdated.Quantity * orderItemUpdated.UnitValue;

            // Act
            order.UpdateItem(orderItemUpdated);

            // Assert
            Assert.Equal(totalPedido, order.TotalValue);
        }

        [Fact(DisplayName = "Update Units ExistingItem with Units above allowed")]
        [Trait("Category", "Order Tests")]
        public void UpdateItemOrder_UpdateUnitsItemExistingAboveAllowed_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem = new OrderItem(productId, "Product Test", 1, 100);
            order.AddItem(orderItem);

            var orderItemUpdated = new OrderItem(productId, "Product Test", Order.MAX_UNITS_ITEM + 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.UpdateItem(orderItemUpdated));
        }

        [Fact(DisplayName = "Remove Item Order Nonexistent")]
        [Trait("Category", "Order Tests")]
        public void RemoveItemOrder_ItemNonexistent_MustReturnException()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var orderItemRemove = new OrderItem(Guid.NewGuid(), "Product Test", 1, 100);

            // Act & Assert
            Assert.Throws<DomainException>(() => order.RemoveItem(orderItemRemove));
        }


        [Fact(DisplayName = "Remove Item  Must Calculate Total Value")]
        [Trait("Category", "Order Tests")]
        public void RemoveItemOrder_ItemExistent_MustUpdateTotalValue()
        {
            // Arrange
            var order = Order.OrderFactory.NewOrderDraft(Guid.NewGuid());
            var productId = Guid.NewGuid();
            var orderItem1 = new OrderItem(Guid.NewGuid(), "Product XYZ", 2, 100);
            var orderItem2 = new OrderItem(productId, "Product Test", 3, 15);
            order.AddItem(orderItem1);
            order.AddItem(orderItem2);

            var totalOrder = orderItem2.Quantity * orderItem2.UnitValue;

            // Act
            order.RemoveItem(orderItem1);

            // Assert
            Assert.Equal(totalOrder, order.TotalValue);
        }
    }
}
