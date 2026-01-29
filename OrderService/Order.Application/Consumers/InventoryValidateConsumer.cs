using Contracts.Events.Orders;
using MassTransit;
using MediatR;
using Order.Application.Dto.Orders.UpdateStatus;
using Order.Application.Features.Orders.UpdateStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Consumers
{
    public sealed class InventoryValidatedConsumer : IConsumer<InventoryValidatedEvent>
    {
        private readonly IMediator _mediator;

        public InventoryValidatedConsumer(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task Consume(ConsumeContext<InventoryValidatedEvent> context)
        {
            var inventoryValidatedEvent = context.Message;

            var cmd = new UpdateOrderStatusCommand(
                inventoryValidatedEvent.OrderId,
                inventoryValidatedEvent.Approved,
                inventoryValidatedEvent.Failures.Select(f => new UpdateOrderStockFailureDto(
                    f.Sku,
                    f.Requested,
                    f.Available
                )).ToList()
            );

            await _mediator.Send(cmd, context.CancellationToken);
        }
    }
}
