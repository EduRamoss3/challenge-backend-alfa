using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.GetById
{
    public sealed record OrderLineDto
    {
        public string Sku { get; init; } = default!;
        public int Quantity { get; init; }
    }
}
