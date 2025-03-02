using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class OrderStatus : AuditableEntity {
    public Guid OrderId {get;set;}
    public int Status {get;set;} = (int)OrderStatusEnum.Pending;
    public string Description {get;set;} = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;

    public Order Order {get;set;} = null!;
}

public enum OrderStatusEnum {
    Pending,
    Processing,
    Shipped,
    Delivered,
    Cancelled
}