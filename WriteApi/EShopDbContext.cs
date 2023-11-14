using Microsoft.EntityFrameworkCore;
using WriteApi.Entities;

namespace WriteApi;

public class EShopDbContext : DbContext
{
    public DbSet<UserEntity> Users { get; init; } = null!;
    public DbSet<OrderEntity> Orders { get; init; } = null!;
    public DbSet<OrderItemEntity> OrderItems { get; init; } = null!;
    public DbSet<ProductEntity> Products { get; init; } = null!;
    
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<UserEntity>().HasIndex(user => user.Email).IsUnique();
        
        modelBuilder.Entity<UserEntity>()
            .HasMany(user => user.Orders)
            .WithOne(order => order.User)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<OrderEntity>()
            .HasMany(order => order.OrderItems)
            .WithOne(orderItem => orderItem.Order)
            .OnDelete(DeleteBehavior.Cascade);
    }
}