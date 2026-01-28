using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events.Orders
{
    public sealed record StockFailureItem
    {
        public required string Sku { get; init; }
        public required int Requested { get; init; }
        public required int Available { get; init; }
    }
}
