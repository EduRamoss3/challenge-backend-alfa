using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.Add
{
    public sealed record AddOrderDto
    {
        public required List<AddOrderLineDto> Lines { get; init; }
    }
}
