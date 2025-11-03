using Inventory.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Role> Roles => Set<Role>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Warehouse> Warehouses => Set<Warehouse>();
    public DbSet<Shelf> Shelves => Set<Shelf>();
    public DbSet<Material> Materials => Set<Material>();
    public DbSet<ShelfMaterial> ShelfMaterials => Set<ShelfMaterial>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder b)
    {
        
        b.Entity<Role>().ToTable("roles");
        b.Entity<User>().ToTable("users");
        b.Entity<Warehouse>().ToTable("warehouse");
        b.Entity<Shelf>().ToTable("shelves");
        b.Entity<Material>().ToTable("materials");
        b.Entity<ShelfMaterial>().ToTable("shelf_material");
        b.Entity<Transaction>().ToTable("transactions");

        
        b.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        b.Entity<User>()
            .Property(u => u.IsActive)
            .HasDefaultValue(true);

        
        b.Entity<Shelf>()
            .HasOne(s => s.Warehouse)
            .WithMany(w => w.Shelves)
            .HasForeignKey(s => s.WarehouseId)
            .OnDelete(DeleteBehavior.Cascade);

        
        b.Entity<Material>()
            .Property(m => m.DoorType).HasMaxLength(20);
        b.Entity<Material>()
            .Property(m => m.UnitOfMeasure).HasMaxLength(20);
        b.Entity<Material>()
            .Property(m => m.Description).HasMaxLength(30);

        
        b.Entity<ShelfMaterial>()
            .HasOne(sm => sm.Shelf)
            .WithMany(s => s.ShelfMaterials)
            .HasForeignKey(sm => sm.ShelfId)
            .OnDelete(DeleteBehavior.Cascade);

        b.Entity<ShelfMaterial>()
            .HasOne(sm => sm.Material)
            .WithMany(m => m.ShelfMaterials)
            .HasForeignKey(sm => sm.MaterialId)
            .OnDelete(DeleteBehavior.Restrict);

        
        b.Entity<Transaction>()
            .HasOne(t => t.Material)
            .WithMany(m => m.Transactions)
            .HasForeignKey(t => t.MaterialId);

        b.Entity<Transaction>()
            .HasOne(t => t.Shelf)
            .WithMany()
            .HasForeignKey(t => t.ShelfId);

        b.Entity<Transaction>()
            .HasOne(t => t.User)
            .WithMany(u => u.Transactions)
            .HasForeignKey(t => t.UserId);

        base.OnModelCreating(b);
    }
}
