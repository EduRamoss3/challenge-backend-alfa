using Order.Domain.Entities.Orders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Interfaces.Repositories
{
    public interface IItemRepository
    {
        Task AddAsync(Item item, CancellationToken ct);
    }
}
