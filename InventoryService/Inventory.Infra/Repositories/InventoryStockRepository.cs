using Inventory.Domain.Entities;
using Inventory.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using Order.Infra.Context;

namespace Inventory.Infra.Repositories
{
    public sealed class InventoryItemStockRepository : IInventoryItemStockRepository
    {
        private readonly InventoryDbContext _db;

        public InventoryItemStockRepository(InventoryDbContext db)
        {
            _db = db;
        }

        public Task<InventoryStock?> GetBySkuAsync(string sku, CancellationToken ct)
        {
            return _db.InventoryStocks
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Sku == sku, ct);
        }

        public async Task AddAsync(InventoryStock stock, CancellationToken ct)
        {
            await _db.InventoryStocks.AddAsync(stock, ct);
        }
    }
}
