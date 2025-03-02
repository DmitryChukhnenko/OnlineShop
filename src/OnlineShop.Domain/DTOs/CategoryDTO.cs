namespace OnlineShop.Domain.DTOs; 

public class CategoryDTO : AuditableEntityDTO {
    public string Name { get; set; } = string.Empty;
    public string Slug { get; set; } = string.Empty;
    public Guid? ParentCategoryId { get; set; }
    public List<Guid> ChildCategories { get; set; } = new();
}