using Microsoft.EntityFrameworkCore;
using PDBProject.WriteApi.Entities;

namespace PDBProject.WriteApi;

public class EShopDbContext : DbContext
{
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; init; } = null!;
    public DbSet<OrderEntity> Orders { get; init; } = null!;
    public DbSet<OrderItemEntity> OrderItems { get; init; } = null!;
    public DbSet<ProductEntity> Products { get; init; } = null!;

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

        modelBuilder.Entity<OrderItemEntity>()
            .HasOne(orderItem => orderItem.Product);
    }
}