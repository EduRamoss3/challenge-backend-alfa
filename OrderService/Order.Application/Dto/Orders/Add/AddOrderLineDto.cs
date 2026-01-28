using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.Add
{
    public sealed record AddOrderLineDto
    {
        public required string Sku { get; init; }
        public required int Quantity { get; init; }
    }
}
