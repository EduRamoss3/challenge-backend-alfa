using Order.Domain.Enums.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.GetById
{
    public sealed record OrderDetailsDto
    {
        public Guid Id { get; init; }
        public DateTime CreatedAt { get; init; }
        public Status Status { get; init; }

        public List<OrderLineDto> Lines { get; init; } = new();
        public List<StockFailureDto> StockFailures { get; init; } = new();
    }
}
