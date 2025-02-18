using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities; 

public class ProductReview : AuditableEntity {
    [Key]
    public Guid Id {get; set;}
    public Guid UserId {get; set;}
    public Guid ProductId {get;set;}
    public int Rate {get; set;}
    public string Description {get; set;} = string.Empty;

    public ApplicationUser User {get;set;} = null!;
    public Product Product {get;set;} = null!;
}