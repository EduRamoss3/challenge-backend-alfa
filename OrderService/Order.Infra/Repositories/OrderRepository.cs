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
}
