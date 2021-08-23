using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Moq;
using Moq.AutoMock;
using PlanetStore.Sales.Application.Commands;
using PlanetStore.Sales.Domain;
using PlanetStore.Sales.Domain.Interfaces;
using Xunit;

namespace PlanetStore.Sales.Application.Tests.Orders
{
    public class OrderCommandHandlerTests
    {
        private readonly Guid _customerId;
        private readonly Guid _productId;

        public OrderCommandHandlerTests()
        {
            _customerId = Guid.NewGuid();
            _productId = Guid.NewGuid();
        }

        [Fact(DisplayName = "Add Item new Order with success")]
        [Trait("Category", "Order Command Handler")]
        public async Task AddItem_NewOrder_MustBeExecutedWiyhSuccess()
        {
            // Arrange
            var orderCommand = new AddItemOrderCommand(_customerId, _productId, "Product Test", 2, 100);

            var mocker = new AutoMocker();
            var orderHandler = mocker.CreateInstance<OrderCommandHandler>();

            mocker.GetMock<IOrderRepository>().Setup(r => r.UnitOfWork.Commit()).Returns(Task.FromResult(true));

            // Act
            var result = await orderHandler.Handle(orderCommand, CancellationToken.None);

            // Assert
            Assert.True(result);
            mocker.GetMock<IOrderRepository>().Verify(r => r.Add(It.IsAny<Order>()), Times.Once);
            mocker.GetMock<IOrderRepository>().Verify(r => r.UnitOfWork.Commit(), Times.Once);
            //mocker.GetMock<IMediator>().Verify(r => r.Publish(It.IsAny<INotification>(), CancellationToken.None), Times.Once);
        }
    }
}
