namespace OnlineShop.Domain.Entities; 

public abstract class AuditableEntity
{
    public Guid Id { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}