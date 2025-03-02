using Microsoft.EntityFrameworkCore;
using OnlineShop.Application.Interfaces;
using OnlineShop.Domain.Entities;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Infrastructure.Common;

public class Repository<T> : IRepository<T> where T : AuditableEntity
{
    private readonly ApplicationDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(ApplicationDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public IQueryable<T> Query() => _dbSet.AsQueryable();

    public async Task<T>? GetByIdAsync(Guid id) => 
        await _dbSet.FirstOrDefaultAsync(e => e.Id == id);

    public async Task AddAsync(T entity) => 
        await _dbSet.AddAsync(entity);

    public async Task UpdateAsync(T entity) => 
        await Task.Run(() => _dbSet.Update(entity));

    public async Task SoftDeleteAsync(Guid id)
    {
        var entity = await GetByIdAsync(id)!;
        if (entity != null) {
            entity.IsDeleted = true;
            await UpdateAsync(entity);
        }
    }

}