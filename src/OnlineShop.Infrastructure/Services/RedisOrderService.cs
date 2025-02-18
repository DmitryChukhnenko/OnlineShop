using System.Text.Json;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Services;

// Infrastructure/Services/RedisOrderService.cs
public class RedisOrderService : IOrderService
{
    private readonly StackExchange.Redis.IDatabase _redis;
    private readonly TimeSpan _expiry = TimeSpan.FromDays(7);

    public RedisOrderService(StackExchange.Redis.IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public async Task<Order?> GetOrderAsync(Guid Id)
    {
        var data = await _redis.StringGetAsync(GetKey(Id));
        return data.IsNullOrEmpty ? null : JsonSerializer.Deserialize<Order>(data!);
    }

    public async Task<Order> UpdateOrderAsync(Order Order)
    {
        await _redis.StringSetAsync(GetKey(Order.Id), 
            JsonSerializer.Serialize(Order), 
            _expiry);
        return Order;
    }

    private static string GetKey(Guid Id) => $"Order:{Id}";
}

public interface IOrderService
{
    public Task<Order?> GetOrderAsync(Guid Id);
    public Task<Order> UpdateOrderAsync(Order Order);

}