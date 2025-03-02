namespace OnlineShop.Domain.DTOs; 

public class ProductReviewDTO : AuditableEntityDTO {
    public int Rate {get; set;}
    public string Description {get; set;} = string.Empty;
    public ApplicationUserDTO User {get; set;} = null!;
    public ProductDTO Product {get; set;} = null!;
}