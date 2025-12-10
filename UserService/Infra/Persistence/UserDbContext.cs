using EnrollmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;

namespace EnrollmentService.Infrastructure.Persistence;

public class EnrollmentDbContext : DbContext
{
    public EnrollmentDbContext(DbContextOptions<EnrollmentDbContext> options) : base(options) { }
    public DbSet<Enrollment> Enrollments => Set<Enrollment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Enrollment>(b =>
        {
            b.HasKey(x => x.Id);
            b.Property(x => x.ItemSku).HasMaxLength(100).IsRequired();
            b.Property(x => x.Status).HasMaxLength(20).HasDefaultValue("Pending");
            b.Property(x => x.PriceLocked).HasColumnType("numeric(18,2)");
        });
    }
}