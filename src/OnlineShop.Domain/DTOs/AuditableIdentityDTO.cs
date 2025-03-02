namespace OnlineShop.Domain.DTOs; 

public abstract class AuditableEntityDTO
{
    public bool IsDeleted { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? UpdatedAt { get; set; }
}