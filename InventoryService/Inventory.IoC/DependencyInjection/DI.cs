using Inventory.Domain.Interfaces.Repositories;
using Inventory.Domain.Interfaces.UnitOfWork;
using Inventory.Infra.Repositories;
using Inventory.Infra.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;
using Inventory.Application.Features.Stock.Validate;
using Inventory.Infra.Context;
using Inventory.Application.Consumers;
using FluentValidation;

namespace Inventory.IoC.DependencyInjection
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<InventoryDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IInventoryItemStockRepository, InventoryItemStockRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(ValidateOrderStockCommand).Assembly));
            services.AddTransient<IValidator<ValidateOrderStockCommand>, ValidateOrderStockCommandValidator>();
            services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();

                x.AddConsumer<OrderCreatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:Host"], h =>
                    {
                        h.Username(configuration["RabbitMq:User"] ?? throw new KeyNotFoundException("RabbitMq user not found"));
                        h.Password(configuration["RabbitMq:Pass"] ?? throw new KeyNotFoundException("RabbitMq pass not found"));
                    });

                    cfg.ReceiveEndpoint(
                        configuration["RabbitMq:QueueOrder"] ?? throw new KeyNotFoundException("RabbitMq queue not found"),
                        e =>
                        {
                            e.ConfigureConsumer<OrderCreatedConsumer>(context);
                        });
                });
            });
            return services;
        }
    }
}
