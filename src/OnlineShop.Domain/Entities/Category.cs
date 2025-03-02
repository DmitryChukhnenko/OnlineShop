using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class Category : AuditableEntity {
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    
    public Category? ParentCategory { get; set; }
    public List<Category> ChildCategories { get; set; } = [];
    public List<ProductCategory> ProductCategories { get; set; } = [];
}