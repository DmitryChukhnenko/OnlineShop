namespace OnlineShop.Domain.DTOs; 

public class OrderDTO : AuditableEntityDTO {
    public Guid Id { get; set; }
    public decimal TotalAmount { get; set; }
    public string ShippingAddress { get; set; } = string.Empty;
    public DateTime? ShippedDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public ICollection<OrderItemDTO> Items { get; set; } = [];
}