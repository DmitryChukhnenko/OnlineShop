using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class OrderStatus : AuditableEntity {
    [Key]
    public Guid Id {get;set;}
    public Guid OrderId {get;set;}
    public string Status {get;set;} = string.Empty;
    public string Description {get;set;} = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;

    public Order Order {get;set;} = null!;
}