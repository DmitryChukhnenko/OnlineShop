using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class Category : AuditableEntity {
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    
    public ICollection<ProductCategory> ProductCategories { get; set; } = [];
}