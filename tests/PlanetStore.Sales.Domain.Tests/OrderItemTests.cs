using System;
using PlanetStore.Core.DomainObjects;
using Xunit;

namespace PlanetStore.Sales.Domain.Tests
{
    public class OrderItemTests
    {
        [Fact(DisplayName = "New Item with Units below allowed")]
        [Trait("Category", "OrderItem Tests")]
        public void NewItemOrder_UnitsItemBelowAllowed_MustReturnException()
        {
            Assert.Throws<DomainException>(() => new OrderItem(Guid.NewGuid(), "Product Test", Order.MIN_UNITS_ITEM - 1, 100));
        }
    }
}
