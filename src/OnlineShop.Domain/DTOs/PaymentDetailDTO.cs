namespace OnlineShop.Domain.DTOs; 

public class PaymentDetailDTO : AuditableEntityDTO  {
    public decimal Amount {get; set;}
    public string Currency {get; set;} = string.Empty;
    public string CardNumber {get; set;} = string.Empty;
    public string CardHolderName {get; set;} = string.Empty;
    public DateTime ExpirationDate {get; set;}
    public string CVV {get; set;} = string.Empty;

    public Guid OrderId {get; set;}
}