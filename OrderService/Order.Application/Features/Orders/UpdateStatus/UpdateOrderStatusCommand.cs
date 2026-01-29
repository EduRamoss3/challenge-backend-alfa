using MediatR;
using Order.Application.Dto.Orders.UpdateStatus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Features.Orders.UpdateStatus
{
    public sealed record UpdateOrderStatusCommand(
        Guid OrderId,
        bool Approved,
        IReadOnlyList<UpdateOrderStockFailureDto> Failures
    ) : IRequest;
}
