using System.Text.Json;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Services;

public class RedisOrderService : IOrderService
{
    private readonly StackExchange.Redis.IDatabase _redis;
    private readonly TimeSpan _expiry = TimeSpan.FromDays(7);

    public RedisOrderService(StackExchange.Redis.IConnectionMultiplexer redis)
    {
        _redis = redis.GetDatabase();
    }

    public void AddItem(Order order, Product product, int quantity)
    {
        if (product.StockQuantity < quantity)
            throw new InvalidOperationException("Not enough stock available for this product.");
        if (quantity <= 0)
            throw new InvalidOperationException("Quantity must be greater than zero.");
        
        order.Items.Add(new OrderItem
        {
            ProductId = product.Id,
            Quantity = quantity,
            UnitPrice = product.Price
        });
        
        product.StockQuantity -= quantity;
    }
    
    public void RemoveItem(Order order, Guid itemId)
    {
        var item = order.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null) return;
        
        order.Items.Remove(item);
        item.Product.StockQuantity += item.Quantity;
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
