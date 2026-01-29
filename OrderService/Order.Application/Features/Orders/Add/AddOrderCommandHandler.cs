using Contracts.Events.Orders;
using FluentValidation;
using MassTransit;
using MediatR;
using Order.Application.Dto.Orders;
using Order.Application.Dto.Orders.Add;
using Order.Domain.Entities.Orders;
using Order.Domain.Enums.Orders;
using Order.Domain.Exceptions;
using Order.Domain.Interfaces.UnitOfWork;
using Order.Domain.ValueObjects.Orders;

namespace Order.Application.Features.Orders.Add
{
    internal sealed class AddOrderCommandHandler
        : IRequestHandler<AddOrderCommand, AddOrderResultDto>
    {
        private readonly IUnitOfWork _uow;
        private readonly IValidator<AddOrderCommand> _validator;

        private readonly IPublishEndpoint _publish;

        public AddOrderCommandHandler(IUnitOfWork uow, IPublishEndpoint publish, IValidator<AddOrderCommand> validator)
        {
            _validator = validator;
            _uow = uow;
            _publish = publish;
        }

        public async Task<AddOrderResultDto> Handle(AddOrderCommand request, CancellationToken ct)
        {
            await _validator.ValidateAndThrowAsync(request, ct);

            var lines = request.Payload.Lines.Select(orderLine =>
                new OrderLine(
                    new Sku(orderLine.Sku),
                    new Quantity(orderLine.Quantity)
                )).ToList();

            var order = new Domain.Entities.Orders.Order(lines);

            await _uow.Orders.AddAsync(order, ct);
            await _uow.CommitAsync(ct);

            await _publish.Publish(new OrderCreatedEvent
            {
                OrderId = order.Id,
                Lines = lines.Select(x => new OrderCreatedLine
                {
                    Sku = x.Sku.Value,
                    Quantity = x.Quantity.Value
                }).ToList()
            }, ct);


            return new AddOrderResultDto
            {
                OrderId = order.Id,
                Status = order.Status
            };
        }
    }
}
