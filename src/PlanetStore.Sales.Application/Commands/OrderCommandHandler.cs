using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using PlanetStore.Sales.Application.Events;
using PlanetStore.Sales.Domain;
using PlanetStore.Sales.Domain.Interfaces;

namespace PlanetStore.Sales.Application.Commands
{
    public class OrderCommandHandler :
        IRequestHandler<AddItemOrderCommand, bool>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMediator _mediator;

        public OrderCommandHandler(IOrderRepository orderRepository, IMediator mediator)
        {
            _orderRepository = orderRepository;
            _mediator = mediator;
        }

        public async Task<bool> Handle(AddItemOrderCommand message, CancellationToken cancellationToken)
        {
            var orderItem = new OrderItem(message.ProductId, message.ProductName, message.Quantity, message.UnitValue);
            var order = Order.OrderFactory.NewOrderDraft(message.CustomerId);
            order.AddItem(orderItem);

            _orderRepository.Add(Order.OrderFactory.NewOrderDraft(message.CustomerId));

            await _mediator.Publish(new OrderItemAddedEvent(
                order.CustomerId, order.Id, message.ProductId, message.ProductName, message.Quantity, message.UnitValue), cancellationToken);

            return true;
        }
    }
}
