using Order.Domain.Common;
using Order.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Interfaces.Repositories
{
    public interface IOrderRepository
    {
        Task AddAsync(Domain.Entities.Orders.Order order, CancellationToken ct);
        Task<Domain.Entities.Orders.Order?> GetById(Guid orderId);
    }
}
