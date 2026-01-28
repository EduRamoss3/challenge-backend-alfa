using Microsoft.EntityFrameworkCore;
using Order.Domain.Entities.Orders;

namespace Order.Infra.Context
{
    public sealed class OrderDbContext : DbContext
    {
        public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options) { }


        public DbSet<Domain.Entities.Orders.Order> Orders => Set<Domain.Entities.Orders.Order>();
        public DbSet<Item> Items => Set<Item>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(OrderDbContext).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
