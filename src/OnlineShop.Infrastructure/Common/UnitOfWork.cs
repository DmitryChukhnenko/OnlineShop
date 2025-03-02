using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Infrastructure.Common;

public class UnitOfWork : IUnitOfWork, IDisposable
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        
        Products = new Repository<Product>(context);
        Orders = new Repository<Order>(context);
        Categories = new Repository<Category>(context);
        OrderStatuses = new Repository<OrderStatus>(context);
        PaymentDetails = new Repository<PaymentDetail>(context);
        ProductReviews = new Repository<ProductReview>(context);
    }

    public IRepository<Product> Products { get; }
    public IRepository<Order> Orders { get; }
    public IRepository<Category> Categories { get; }
    public IRepository<OrderStatus> OrderStatuses { get; }
    public IRepository<PaymentDetail> PaymentDetails { get; }
    public IRepository<ProductReview> ProductReviews { get; }

    IRepository<Product> IUnitOfWork.Products => new Repository<Product>(_context);
    IRepository<Order> IUnitOfWork.Orders => new Repository<Order>(_context);
    IRepository<Category> IUnitOfWork.Categories => new Repository<Category>(_context);
    IRepository<OrderStatus> IUnitOfWork.OrderStatuses => new Repository<OrderStatus>(_context);
    IRepository<PaymentDetail> IUnitOfWork.PaymentDetails => new Repository<PaymentDetail>(_context);
    IRepository<ProductReview> IUnitOfWork.ProductReviews => new Repository<ProductReview>(_context);

    public async Task<int> CommitAsync() =>
        await _context.SaveChangesAsync();

    public void Dispose() => _context.Dispose();
}