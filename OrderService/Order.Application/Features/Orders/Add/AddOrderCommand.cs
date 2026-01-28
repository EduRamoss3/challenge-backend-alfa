using MediatR;
using Order.Application.Dto.Orders.Add;

namespace Order.Application.Features.Orders.Add
{
    public sealed record AddOrderCommand(AddOrderDto Payload)
        : IRequest<AddOrderResultDto>;
}
