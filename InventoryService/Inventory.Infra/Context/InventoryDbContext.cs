using Inventory.Domain.Entities;
using Inventory.Domain.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Infra.Context
{
    public sealed class InventoryDbContext : DbContext
    {
        public InventoryDbContext(DbContextOptions<InventoryDbContext> options) : base(options) { }

        public DbSet<InventoryStock> InventoryStocks => Set<InventoryStock>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(InventoryDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }

        /// <summary>
        /// Seeds initial stock records for local/dev testing.
        /// Idempotent: if there is any stock already, it does nothing.
        /// </summary>
        public async Task SeedTestItemsAsync(CancellationToken ct = default)
        {
            // If there's already any stock data, don't seed again.
            if (await InventoryStocks.AsNoTracking().AnyAsync(ct))
                return;

            var items = new List<InventoryStock>
            {
                // Enough stock
                new InventoryStock(new Sku("SKU-001"), new StockQuantity(100)),
                new InventoryStock(new Sku("SKU-002"), new StockQuantity(25)),
                new InventoryStock(new Sku("SKU-003"), new StockQuantity(10)),

                // Edge cases for testing
                new InventoryStock(new Sku("SKU-LOW"), new StockQuantity(1)),
                new InventoryStock(new Sku("SKU-ZERO"), new StockQuantity(0))
            };
            Console.WriteLine("Adding test itens seed...");

            await InventoryStocks.AddRangeAsync(items, ct);
            await SaveChangesAsync(ct);
        }
    }
}
