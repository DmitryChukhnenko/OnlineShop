using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class Order : AuditableEntity {
    [Key]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public Guid PaymentId {get; set;}
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public DateTime? ShippedDate { get; set; }

    public ApplicationUser User { get; set; } = null!;
    public ICollection<OrderStatus> Statuses { get; set; } = [];
    public PaymentDetail Payment { get; set; } = null!;
    public ICollection<OrderItem> Items { get; set; } = [];
}