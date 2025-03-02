using OnlineShop.Domain.Entities;

namespace OnlineShop.Application.Interfaces;

public interface IUnitOfWork
{
    IRepository<Product> Products { get; }
    IRepository<Order> Orders { get; }
    IRepository<Category> Categories { get; }
    IRepository<OrderStatus> OrderStatuses { get; }
    IRepository<PaymentDetail> PaymentDetails { get; }
    IRepository<ProductReview> ProductReviews { get; }

    Task<int> CommitAsync();
}