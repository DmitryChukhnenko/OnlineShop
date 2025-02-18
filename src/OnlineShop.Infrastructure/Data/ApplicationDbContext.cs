using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineShop.Domain.Entities;

namespace OnlineShop.Infrastructure.Data;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid> 
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema("public"); // Установка схемы по умолчанию
        
        // Глобальный фильр для мягкого удаления        
        modelBuilder.Entity<Product>().HasQueryFilter(p => !p.IsDeleted);
        modelBuilder.Entity<Category>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<Order>().HasQueryFilter(o => !o.IsDeleted);
        modelBuilder.Entity<ApplicationUser>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<PaymentDetail>().HasQueryFilter(pd => !pd.IsDeleted);
        modelBuilder.Entity<OrderStatus>().HasQueryFilter(os => !os.IsDeleted);
        modelBuilder.Entity<OrderItem>().HasQueryFilter(oi => !oi.IsDeleted);
        modelBuilder.Entity<ProductReview>().HasQueryFilter(pr => !pr.IsDeleted); 

        // Конфигурация отношений
        modelBuilder.Entity<ProductReview>()
            .HasOne(pr => pr.User)
            .WithMany(u => u.Reviews)
            .HasForeignKey(pr => pr.UserId);
        modelBuilder.Entity<ProductReview>()
            .HasOne(pr => pr.Product)
            .WithMany(p => p.Reviews)
            .HasForeignKey(pr => pr.ProductId);

        modelBuilder.Entity<ProductCategory>()
            .HasKey(pc => new { pc.ProductId, pc.CategoryId });

        modelBuilder.Entity<PaymentDetail>()
            .HasOne(pd => pd.Order)
            .WithOne(o => o.Payment)
            .HasForeignKey<PaymentDetail>(pd => pd.OrderId);
        
        modelBuilder.Entity<OrderStatus>()
            .HasOne(os => os.Order)
            .WithMany(o => o.Statuses)
            .HasForeignKey(os => os.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.Items)
            .HasForeignKey(oi => oi.OrderId);
        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Payment)
            .WithOne(p => p.Order)
            .HasForeignKey<PaymentDetail>(p => p.OrderId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(o => o.UserId);

        // Индексы
        modelBuilder.Entity<Product>()
            .HasIndex(p => new { p.Name, p.SKU })
            .IsUnique();

        modelBuilder.Entity<Category>()
            .HasIndex(c => c.Slug)
            .IsUnique();

        modelBuilder.Entity<ApplicationUser>()
            .HasIndex(u => u.Email)
            .IsUnique();

        // Конфигурация для Value Objects
        modelBuilder.Entity<PaymentDetail>()
            .Property(p => p.Amount)
            .HasColumnType("decimal(18,2)");

        modelBuilder.Entity<Order>()
            .HasIndex(o => o.CreatedAt);

        modelBuilder.Entity<Product>()
            .Property(p => p.Price)
            .HasColumnType("decimal(18,2)")
            .HasConversion<double>();

        modelBuilder.Entity<Order>()
            .Property(o => o.TotalAmount)
            .HasColumnType("decimal(18,2)");
    }

    public DbSet<Product> Products => Set<Product>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<ProductCategory> ProductCategories => Set<ProductCategory>();
    public DbSet<ProductReview> ProductReviews => Set<ProductReview>();
    public DbSet<PaymentDetail> PaymentDetails => Set<PaymentDetail>();
    public DbSet<OrderStatus> OrderStatuses => Set<OrderStatus>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
}