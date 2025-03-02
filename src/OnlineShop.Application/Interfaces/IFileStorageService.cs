namespace OnlineShop.Application.Interfaces;
public interface IFileStorageService {
    public Task<string> UploadProductImage(Stream fileStream, string fileName);
}