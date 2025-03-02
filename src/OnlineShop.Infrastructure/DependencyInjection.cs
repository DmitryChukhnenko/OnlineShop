using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OnlineShop.Application.Interfaces;
using OnlineShop.Infrastructure.Services;
using StackExchange.Redis;

namespace OnlineShop.Infrastructure;
public static class DependencyInjectionMethods {
    
    public static IServiceCollection AddRedisServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddStackExchangeRedisCache(options =>
        {
            options.Configuration = config.GetConnectionString("Redis");
            options.InstanceName = "OnlineShop_";
        });
        
        services.AddSingleton<IConnectionMultiplexer>(_ => 
            ConnectionMultiplexer.Connect(config.GetConnectionString("Redis")!));
        
        services.AddScoped<IOrderService, RedisOrderService>();
        return services;
    }

}