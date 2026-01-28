using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Domain.Entities.Orders;

namespace Order.Infra.Context.Configurations
{
    public sealed class ItemEntityConifg : IEntityTypeConfiguration<Item>
    {
        public void Configure(EntityTypeBuilder<Item> builder)
        {
            builder.ToTable("items");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Name)
                .HasMaxLength(300)
                .IsRequired();

            builder.Property(x => x.Description)
                .HasMaxLength(300)
                .IsRequired();
        }
    }
}
