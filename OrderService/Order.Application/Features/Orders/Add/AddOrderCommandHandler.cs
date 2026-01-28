using MediatR;
using Order.Application.Dto.Orders;
using Order.Application.Dto.Orders.Add;
using Order.Domain.Entities.Orders;
using Order.Domain.Enums.Orders;
using Order.Domain.Exceptions;
using Order.Domain.Interfaces.UnitOfWork;
using Order.Domain.Records.Orders;

namespace Order.Application.Features.Orders.Add
{
    internal sealed class AddOrderCommandHandler
        : IRequestHandler<AddOrderCommand, AddOrderResultDto>
    {
        private readonly IUnitOfWork _uow;

        public AddOrderCommandHandler(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<AddOrderResultDto> Handle(AddOrderCommand request, CancellationToken ct)
        {
            var lines = request.Payload.Lines.Select(orderLine =>
                new OrderLine(
                    new Sku(orderLine.Sku),
                    new Quantity(orderLine.Quantity)
                )).ToList();

            var order = new Domain.Entities.Orders.Order(lines);

            await _uow.Orders.AddAsync(order, ct);
            await _uow.CommitAsync(ct);

            return new AddOrderResultDto
            {
                OrderId = order.Id,
                Status = order.Status
            };
        }
    }
}
