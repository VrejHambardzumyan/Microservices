using CatalogService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace CatalogService.Infrastructure.Persistence;

public class CatalogDbContext : DbContext
{
    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options) { }

    public DbSet<Item> Items => Set<Item>();
    public DbSet<Price> Prices => Set<Price>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Item>(b =>
        {
            b.HasKey(x => x.Id);
            b.HasIndex(x => x.Sku).IsUnique();
            b.Property(x => x.Name).HasMaxLength(200).IsRequired();
            b.HasMany(x => x.Prices).WithOne(x => x.Item!).HasForeignKey(x => x.ItemId);
        });

        modelBuilder.Entity<Price>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.Amount).HasColumnType("numeric(18,2)");
            b.Property(x => x.Currency).HasMaxLength(3);
        });
    }
}