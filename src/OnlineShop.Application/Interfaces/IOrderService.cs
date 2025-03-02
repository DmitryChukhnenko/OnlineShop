using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Interfaces;

public interface IOrderService {
    public Task<Order?> GetOrderAsync(Guid Id);
    public Task<Order> UpdateOrderAsync(Order Order);
}