using Inventory.Domain.Interfaces.Repositories;
using Inventory.Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Inventory.Infra.Context;


namespace Inventory.Infra.UnitOfWork;

public class UnitOfWork : IUnitOfWork
{
    protected readonly InventoryDbContext _db;
    private readonly ILogger<UnitOfWork> _logger;

    public UnitOfWork(InventoryDbContext db, IInventoryItemStockRepository inventoryItemStockRepository, ILogger<UnitOfWork> logger)
    {
        _db = db ?? throw new ArgumentNullException(nameof(db));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        InventoryItemStockRepository = inventoryItemStockRepository ?? throw new ArgumentNullException(nameof(inventoryItemStockRepository));
    }

    public IInventoryItemStockRepository InventoryItemStockRepository { get; }

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
