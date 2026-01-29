using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Order.Domain.Entities.Orders;
using Order.Domain.ValueObjects.Orders;

namespace Order.Infra.Context.Configurations
{
    public sealed class OrderEntityConfig : IEntityTypeConfiguration<Domain.Entities.Orders.Order>
    {
        public void Configure(EntityTypeBuilder<Domain.Entities.Orders.Order> builder)
        {
            builder.ToTable("orders");
            builder.HasKey(o => o.Id);

            builder.Property(o => o.CreatedAt).IsRequired();

            builder.Property(o => o.Status)
                .HasConversion<int>()
                .IsRequired();

            var skuConverter = new ValueConverter<Sku, string>(
                sku => sku.Value,
                value => new Sku(value));

            var qtyConverter = new ValueConverter<Quantity, int>(
                qty => qty.Value,
                value => new Quantity(value));

            builder.OwnsMany(o => o.Lines, lines =>
            {
                lines.ToTable("order_lines");
                lines.WithOwner().HasForeignKey("OrderId");

                lines.Property<Guid>("Id");
                lines.HasKey("Id");

                lines.Property(l => l.Sku)
                    .HasConversion(skuConverter)
                    .HasColumnName("Sku")
                    .HasMaxLength(64)
                    .IsRequired();

                lines.Property(l => l.Quantity)
                    .HasConversion(qtyConverter)
                    .HasColumnName("Quantity")
                    .IsRequired();

                lines.HasIndex("OrderId", "Sku").IsUnique();
            });

            builder.Navigation(o => o.Lines)
                .UsePropertyAccessMode(PropertyAccessMode.Field);

            builder.OwnsMany(o => o.StockFailures, fails =>
            {
                fails.ToTable("order_stock_failures");
                fails.WithOwner().HasForeignKey("OrderId");

                fails.Property<Guid>("Id");
                fails.HasKey("Id");

                fails.Property(f => f.Sku)
                    .HasConversion(skuConverter)
                    .HasColumnName("Sku")
                    .HasMaxLength(64)
                    .IsRequired();

                fails.Property(f => f.Requested).IsRequired();
                fails.Property(f => f.Available).IsRequired();
            });

            builder.Navigation(o => o.StockFailures)
                .UsePropertyAccessMode(PropertyAccessMode.Field);
        }
    }
}
