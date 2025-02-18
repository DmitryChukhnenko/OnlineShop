using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities;

public class Product : AuditableEntity {
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public int StockQuantity { get; set; }
    public string SKU { get; set; } = string.Empty;
    public string ImageUrl { get; set; } = string.Empty;
    
    public ICollection<ProductCategory> ProductCategories { get; set; } = [];
    public ICollection<OrderItem> OrderItems { get; set; } = [];
    public ICollection<ProductReview> Reviews { get; set; } = [];
}