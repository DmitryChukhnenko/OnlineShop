using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class OrderItem : AuditableEntity {
    [Key]
    public Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }

    public Order Order { get; set; } = null!;
    public Product Product { get; set; } = null!;
}