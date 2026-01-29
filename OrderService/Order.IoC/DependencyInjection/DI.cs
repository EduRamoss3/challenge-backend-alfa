using FluentValidation;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Consumers;
using Order.Application.Features.Orders.Add;
using Order.Application.Features.Orders.UpdateStatus; 
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.UnitOfWork;
using Order.Infra.Context;
using Order.Infra.Repositories;
using Order.Infra.UnitOfWork;


namespace Order.IoC.DependencyInjection
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(options =>
                options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddMediatR(cfg =>
                cfg.RegisterServicesFromAssembly(typeof(AddOrderCommand).Assembly));
                services.AddTransient<IValidator<AddOrderCommand>, AddOrderCommandValidation>();
                services.AddMassTransit(x =>
            {
                x.SetKebabCaseEndpointNameFormatter();
               
                x.AddConsumer<InventoryValidatedConsumer>();

                x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(configuration["RabbitMq:Host"], h =>
                    {
                        h.Username(configuration["RabbitMq:User"] ?? throw new KeyNotFoundException("RabbitMq user not found"));
                        h.Password(configuration["RabbitMq:Pass"] ?? throw new KeyNotFoundException("RabbitMq pass not found"));
                    });

                    cfg.ReceiveEndpoint(
                        configuration["RabbitMq:QueueInventoryValidated"]
                        ?? throw new KeyNotFoundException("RabbitMq QueueInventoryValidated not found"),
                        e =>
                        {
                            e.ConfigureConsumer<InventoryValidatedConsumer>(context);
                        });
                });
            });

            return services;
        }
    }
}

          