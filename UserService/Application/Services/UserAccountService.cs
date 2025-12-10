using EnrollmentService.Infrastructure.Persistence;
using EnrollmentService.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using EnrollmentService.Application.Messaging;

namespace EnrollmentService.Application.Services;

public class EnrollmentService
{
    private readonly EnrollmentDbContext _db;
    private readonly EnrollmentPriceRequester _priceRequester;

    public EnrollmentService(EnrollmentDbContext db, EnrollmentPriceRequester priceRequester)
    {
        _db = db;
        _priceRequester = priceRequester;
    }

    public async Task<Guid> CreateEnrollmentAsync(Guid userId, string sku)
    {
        var correlationId = _priceRequester.RequestItemPrice(sku, "USD");

        var enrollment = new Enrollment
        {
            Id = Guid.NewGuid(),
            UserId = userId,
            ItemSku = sku,
            Status = "Pending"
        };

        _db.Entry(enrollment).Property("CorrelationId").CurrentValue = correlationId;

        _db.Enrollments.Add(enrollment);
        await _db.SaveChangesAsync();

        return enrollment.Id;
    }
}