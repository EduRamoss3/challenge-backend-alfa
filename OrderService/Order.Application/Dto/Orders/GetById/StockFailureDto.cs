using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Application.Dto.Orders.GetById
{
    public sealed record StockFailureDto
    {
        public string Sku { get; init; } = default!;
        public int Requested { get; init; }
        public int Available { get; init; }
    }

}
