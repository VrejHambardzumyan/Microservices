using EnrollmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Infrastructure.Persistence.Repositories;

/// <summary>
/// Repository for accessing Enrollment entities.
/// </summary>
public class EnrollmentRepository
{
    private readonly EnrollmentDbContext _db;

    public EnrollmentRepository(EnrollmentDbContext db) => _db = db;

    public async Task<Enrollment?> GetByIdAsync(Guid id) =>
        await _db.Enrollments.FindAsync(id);

    public async Task<Enrollment?> GetByCorrelationIdAsync(Guid correlationId) =>
        await _db.Enrollments.FirstOrDefaultAsync(e => EF.Property<Guid>(e, "CorrelationId") == correlationId);

    public void Add(Enrollment enrollment) => _db.Enrollments.Add(enrollment);

    public Task SaveAsync() => _db.SaveChangesAsync();
}