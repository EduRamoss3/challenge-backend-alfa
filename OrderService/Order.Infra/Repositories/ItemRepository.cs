using Microsoft.Extensions.Logging;
using Order.Domain.Entities.Orders;
using Order.Domain.Interfaces.Repositories;
using Order.Infra.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.Infra.Repositories
{
    public class ItemRepository : IItemRepository
    {
        private readonly OrderDbContext _db;
        private readonly ILogger<ItemRepository> _logger;

        public ItemRepository(OrderDbContext db, ILogger<ItemRepository> logger)
        {
            _db = db ?? throw new ArgumentNullException(nameof(db));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task AddAsync(Item item, CancellationToken ct)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            await _db.Items.AddAsync(item, ct);
            _logger.LogInformation("Item staged for insertion (not committed yet).");
        }
    }
}
