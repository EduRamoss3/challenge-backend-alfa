using Order.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IOrderRepository Orders { get; }
        Task<int> CommitAsync(CancellationToken ct);
    }
}
