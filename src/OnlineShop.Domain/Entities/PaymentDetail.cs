using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class PaymentDetail : AuditableEntity  {
    public Guid OrderId {get; set;}
    public decimal Amount {get; set;}
    public string Currency {get; set;} = string.Empty;
    public string CardNumber {get; set;} = string.Empty;
    public string CardHolderName {get; set;} = string.Empty;
    public DateTime ExpirationDate {get; set;}
    // No CVV in database
    public string PaymentStatus {get; set;} = string.Empty;

    public Order Order {get; set;} = null!;
}