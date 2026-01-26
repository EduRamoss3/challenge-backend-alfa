using Order.Domain.Entities.Base;
using Order.Domain.Enums.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Entities.Orders
{
    public sealed class Order : Entity
    {
        public DateTime CreatedAt { get; } = DateTime.UtcNow;
        public Status Status { get; private set; } = Status.PENDENTE;

        public void Update(Status status)
        {
            Status = status;
        }

        // EF Core
        public Order() { }
    }
}
