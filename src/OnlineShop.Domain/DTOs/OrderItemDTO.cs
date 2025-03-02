namespace OnlineShop.Domain.DTOs; 

public class OrderItemDTO : AuditableEntityDTO {
    Guid Id { get; set; }
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
    public ProductDTO Product { get; set; } = null!;
}