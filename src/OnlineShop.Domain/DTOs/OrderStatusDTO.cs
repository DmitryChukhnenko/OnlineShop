using OnlineShop.Domain.Entities;

namespace OnlineShop.Domain.DTOs; 

public class OrderStatusDTO : AuditableEntityDTO {
    public OrderStatusEnum Status {get;set;}
    public string Description {get;set;} = string.Empty;
    public string CurrentLocation { get; set; } = string.Empty;
}