using Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventory.Infra.EntityConfiguration
{
    internal sealed class InventoryStockConfig : IEntityTypeConfiguration<InventoryStock>
    {
        public void Configure(EntityTypeBuilder<InventoryStock> builder)
        {
            builder.ToTable("inventory_stock");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Sku)
                .IsRequired()
                .HasMaxLength(64);

            builder.HasIndex(x => x.Sku)
                .IsUnique();

            builder.Property(x => x.Available)
                .IsRequired();
        }
    }
}
