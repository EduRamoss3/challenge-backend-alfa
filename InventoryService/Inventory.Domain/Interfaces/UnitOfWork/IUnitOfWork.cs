using Inventory.Domain.Interfaces.Repositories;


namespace Inventory.Domain.Interfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IInventoryItemStockRepository InventoryItemStockRepository { get; }

        Task<int> CommitAsync(CancellationToken ct);
    }
}
