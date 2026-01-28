using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.UnitOfWork;
using Order.Infra.Context;

namespace Order.Infra.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    protected readonly OrderDbContext _db;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(OrderDbContext db, IOrderRepository orders, ILogger<UnitOfWork> logger, IItemRepository items)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        Orders = orders ?? throw new ArgumentNullException(nameof(orders));
        Items = items;
    }

    public IOrderRepository Orders { get; }
    public IItemRepository  Items { get; }

    public async Task<int> CommitAsync(CancellationToken ct)
    {
        try
        {
            _logger.LogInformation("Committing changes (SaveChangesAsync).");
            var affected = await _db.SaveChangesAsync(ct);
            _logger.LogInformation("Commit succeeded. Rows affected: {RowsAffected}", affected);
            return affected;
        }
        catch (OperationCanceledException ex)
        {
            _logger.LogWarning(ex, "Commit operation was cancelled.");
            throw;
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Commit failed due to DbUpdateException.");
            throw;
        }
    }
}
