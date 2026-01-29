using Inventory.Domain.Entities.Base;
using Inventory.Domain.Exceptions;
using Inventory.Domain.ValueObjects;

namespace Inventory.Domain.Entities;

public sealed class InventoryStock : Entity
{
    public string Sku { get; private set; } = default!;
    public int Available { get; private set; }

    /// <summary>
    /// Creates a stock record for a SKU.
    /// </summary>
    public InventoryStock(Sku sku, StockQuantity available)
    {
        Validate(sku, available);

        Sku = sku.Value;
        Available = available.Value;
    }

    /// <summary>
    /// Adds units to available stock.
    /// </summary>
    public void Increase(StockQuantity quantity)
    {
        DomainValidationException.When(quantity is null, "Increase quantity is required.");

        var newValue = Available + quantity!.Value;
        DomainValidationException.When(newValue < 0, "Invalid resulting stock."); 

        Available = newValue;
    }

    /// <summary>
    /// Tries to reserve stock. Returns false if not enough units.
    /// </summary>
    public bool TryReserve(int requested)
    {
        DomainValidationException.When(requested <= 0, "Requested quantity must be greater than zero.");

        if (Available < requested)
            return false;

        Available -= requested;
        return true;
    }

    private static void Validate(Sku sku, StockQuantity available)
    {
        DomainValidationException.When(sku is null, "SKU is required.");
        DomainValidationException.When(available is null, "Available stock is required.");
    }

    // EF Core
    private InventoryStock() { }
}