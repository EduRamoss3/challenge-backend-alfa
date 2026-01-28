using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Features.Orders.Add;
using Order.Domain.Interfaces.Repositories;
using Order.Domain.Interfaces.UnitOfWork;
using Order.Infra.Context;
using Order.Infra.Repositories;
using Order.Infra.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Order.IoC.DependencyInjection
{
    public static class DI
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<OrderDbContext>(
                options => options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IOrderRepository, OrderRepository>();
            services.AddScoped<IItemRepository, ItemRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(typeof(AddOrderCommand).Assembly));
            return services;
        }

        //public static IServiceCollection AddRateLimitingServices(this IServiceCollection services)
        //{
        //    services.AddRateLimiter(options =>
        //    {
        //        // Rate Limiter Global
        //        options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
        //            RateLimitPartition.GetFixedWindowLimiter("GlobalLimiter",
        //                partition => new FixedWindowRateLimiterOptions
        //                {
        //                    AutoReplenishment = true,
        //                    PermitLimit = 100,
        //                    Window = TimeSpan.FromMinutes(1),
        //                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        //                    QueueLimit = 2
        //                }));
      
        //        // Rate Limiter por IP
        //        options.AddPolicy("IpPolicy", context =>
        //            RateLimitPartition.GetFixedWindowLimiter("IpLimiter",
        //                partition => new FixedWindowRateLimiterOptions
        //                {
        //                    AutoReplenishment = true,
        //                    PermitLimit = 1000,
        //                    Window = TimeSpan.FromMinutes(1),
        //                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
        //                    QueueLimit = 5
        //                }));

        //        // Configuração de resposta quando limite é excedido
        //        options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

        //        // Headers de rate limiting
        //        options.OnRejected = async (context, token) =>
        //        {
        //            context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        //            context.HttpContext.Response.ContentType = "application/json";

        //            var result = JsonSerializer.Serialize(new
        //            {
        //                error = "Too many requests",
        //                message = "Rate limit exceeded. Please try again later.",
        //                retryAfter = context.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter)
        //                    ? retryAfter.TotalSeconds
        //                    : 60
        //            });

        //            await context.HttpContext.Response.WriteAsync(result, token);
        //        };
        //    });

        //    return services;
        //}
    }
}
