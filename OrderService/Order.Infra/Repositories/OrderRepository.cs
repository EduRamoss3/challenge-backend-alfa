using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Domain.Entities.Orders;
using Order.Domain.Interfaces.Repositories;
using Order.Infra.Context;

namespace Order.Infra.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly OrderDbContext _db;
    private readonly ILogger<OrderRepository> _logger;

    public OrderRepository(OrderDbContext db, ILogger<OrderRepository> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task AddAsync(Domain.Entities.Orders.Order order, CancellationToken ct)
    {
        if (order is null) throw new ArgumentNullException(nameof(order));
        await _db.Orders.AddAsync(order, ct);
        _logger.LogInformation("Order staged for insertion (not committed yet).");
    }

    public async Task<Domain.Entities.Orders.Order?> GetById(Guid orderId)
    {
        return await _db.Orders
            .AsNoTracking()
            .Include(o => o.Lines)
            .Include(o => o.StockFailures)
            .AsSplitQuery()
            .FirstOrDefaultAsync(o => o.Id == orderId);
    }
}
