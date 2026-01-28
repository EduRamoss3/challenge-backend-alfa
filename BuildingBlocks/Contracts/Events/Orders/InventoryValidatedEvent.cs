using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Contracts.Events.Orders
{
    public sealed record InventoryValidatedEvent
    {
        public Guid OrderId { get; init; }
        public bool Approved { get; init; }
        public List<StockFailureItem> Failures { get; init; } = new();
    }
}
