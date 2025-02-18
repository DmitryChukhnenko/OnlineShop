using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class PaymentDetail : AuditableEntity  {
    [Key]
    public Guid Id {get; set;}
    public Guid OrderId {get; set;}
    public decimal Amount {get; set;}
    public string Type {get; set;} = string.Empty;

    public Order Order {get; set;} = null!;
}