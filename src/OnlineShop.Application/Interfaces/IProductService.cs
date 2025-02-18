namespace OnlineShop.Application.Interfaces
{
    public interface IProductService
    {
        Task CleanupOldProductsAsync();
    }
}