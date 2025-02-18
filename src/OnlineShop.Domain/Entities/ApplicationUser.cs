using Microsoft.AspNetCore.Identity;

namespace OnlineShop.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public required string FullName { get; set; }
    public string Address { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public bool IsDeleted { get; set; }
    
    public ICollection<Order> Orders { get; set; } = [];
    public ICollection<ProductReview> Reviews { get; set; } = [];
}