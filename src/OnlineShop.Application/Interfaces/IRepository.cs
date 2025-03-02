namespace OnlineShop.Application.Interfaces;

public interface IRepository<T> where T : class
{
    IQueryable<T> Query();
    Task<T>? GetByIdAsync(Guid id);
    Task AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task SoftDeleteAsync(Guid id);
}