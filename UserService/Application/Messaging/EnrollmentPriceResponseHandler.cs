using Common.Messaging.DTO;
using EnrollmentService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace EnrollmentService.Application.Messaging;

public class EnrollmentPriceResponseHandler
{
    private readonly EnrollmentDbContext _db;

    public EnrollmentPriceResponseHandler(EnrollmentDbContext db) => _db = db;

    public async Task HandleAsync(MessageEnvelope<GetItemPriceResponseDto> envelope)
    {
        // Example: find pending enrollment tied to correlationId (simple demo)
        var enrollment = await _db.Enrollments
            .FirstOrDefaultAsync(e => EF.Property<Guid>(e, "CorrelationId") == envelope.CorrelationId);

        if (enrollment == null) return; // Not found, might be logged

        if (envelope.Payload.Found)
        {
            enrollment.PriceLocked = envelope.Payload.Amount;
            enrollment.Currency = envelope.Payload.Currency;
            enrollment.Status = "Confirmed";
        }
        else
        {
            enrollment.Status = "Failed";
        }

        await _db.SaveChangesAsync();
    }
}