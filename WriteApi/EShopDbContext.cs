using Microsoft.EntityFrameworkCore;
using PDBProject.WriteApi.Entities;

namespace PDBProject.WriteApi;

/// <summary>
/// Serves as a context between the PostgreSQL database and the application.
/// </summary>
public class EShopDbContext : DbContext
{
    public EShopDbContext(DbContextOptions<EShopDbContext> options) : base(options)
    {
    }

    public DbSet<UserEntity> Users { get; init; } = null!;
    public DbSet<OrderEntity> Orders { get; init; } = null!;
    public DbSet<OrderItemEntity> OrderItems { get; init; } = null!;
    public DbSet<ProductEntity> Products { get; init; } = null!;

    /// <summary>
    /// Sets up the model constraints when the model is created. Used in migrations.
    /// </summary>
    /// <param name="modelBuilder">ModelBuilder used for the constraints.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // email needs to be unique for all users
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