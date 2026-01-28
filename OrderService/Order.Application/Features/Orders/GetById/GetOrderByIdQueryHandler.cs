using MediatR;
using Order.Application.Dto.Orders.GetById;
using Order.Domain.Interfaces.Repositories;

namespace Order.Application.Features.Orders.GetById
{
    internal sealed class GetOrderByIdQueryHandler : IRequestHandler<GetOrderByIdQuery, OrderDetailsDto?>
    {
        private readonly IOrderRepository _orders;

        public GetOrderByIdQueryHandler(IOrderRepository orders)
        {
            _orders = orders;
        }

        public async Task<OrderDetailsDto?> Handle(GetOrderByIdQuery request, CancellationToken ct)
        {
            var order = await _orders.GetById(request.OrderId);
            if (order is null) return null;

            return new OrderDetailsDto
            {
                Id = order.Id,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                Lines = order.Lines
                    .Select(l => new OrderLineDto
                    {
                        Sku = l.Sku.Value,
                        Quantity = l.Quantity.Value
                    })
                    .ToList(),
                StockFailures = order.StockFailures
                    .Select(f => new StockFailureDto
                    {
                        Sku = f.Sku.Value,
                        Requested = f.Requested,
                        Available = f.Available
                    })
                    .ToList()
            };
        }
    }
}
