using OnlineShop.Application.Interfaces;
using OnlineShop.Infrastructure.Data;

namespace OnlineShop.Infrastructure.Services
{
    public class ProductService : IProductService
    {
        private readonly ApplicationDbContext _context;

        public ProductService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CleanupOldProductsAsync()
        {
            await Task.Delay(10);
            // TODO: Логика очистки старых продуктов
        }
    }
}