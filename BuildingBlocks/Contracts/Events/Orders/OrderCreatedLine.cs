using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events.Orders
{
    public sealed record OrderCreatedLine
    {
        public string Sku { get; init; } = default!;
        public int Quantity { get; init; }
    }

}
