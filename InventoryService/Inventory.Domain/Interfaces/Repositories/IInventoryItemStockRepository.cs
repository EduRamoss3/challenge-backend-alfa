using Inventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Inventory.Domain.Interfaces.Repositories
{
    public interface IInventoryItemStockRepository
    {
        Task<InventoryStock?> GetBySkuAsync(string sku, CancellationToken ct);
        Task AddAsync(InventoryStock stock, CancellationToken ct);
    }
}
