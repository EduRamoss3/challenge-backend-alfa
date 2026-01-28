using MediatR;
using Order.Application.Dto.Orders.GetById;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Orders.GetById
{
    public sealed record GetOrderByIdQuery(Guid OrderId) : IRequest<OrderDetailsDto?>;

}
