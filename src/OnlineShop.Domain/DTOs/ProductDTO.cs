namespace OnlineShop.Domain.DTOs; 

public class ProductDTO : AuditableEntityDTO {
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;

    public ICollection<CategoryDTO> Categories { get; set; } = [];
}