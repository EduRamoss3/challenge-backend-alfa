using Contracts.Events.Orders;
using FluentValidation;
using Inventory.Application.Dto.Stock.Validate;
using Inventory.Application.Features.Stock.Validate;
using MassTransit;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Application.Consumers
{
    public sealed class OrderCreatedConsumer : IConsumer<OrderCreatedEvent>
    {
        private readonly IMediator _mediator;
        private readonly IPublishEndpoint _publish;

        public OrderCreatedConsumer(IMediator mediator, IPublishEndpoint publish)
        {
            _mediator = mediator;
            _publish = publish;
        }

        public async Task Consume(ConsumeContext<OrderCreatedEvent> context)
        {
            var orderCreatedEvent = context.Message;

            var cmd = new ValidateOrderStockCommand(
                orderCreatedEvent.OrderId,
                orderCreatedEvent.Lines.Select(l => new ValidateOrderStockLineDto(l.Sku, l.Quantity)).ToList()
            );

            var result = await _mediator.Send(cmd, context.CancellationToken);

            await _publish.Publish(new InventoryValidatedEvent
            {
                OrderId = result.OrderId,
                Approved = result.Approved,
                Failures = result.Failures.Select(f => new StockFailureItem
                {
                    Sku = f.Sku,
                    Requested = f.Requested,
                    Available = f.Available
                }).ToList()
            }, context.CancellationToken);
        }
    }
}
