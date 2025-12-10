namespace EnrollmentService.Domain.Entities;

public class Enrollment
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string ItemSku { get; set; } = default!;
    public decimal? PriceLocked { get; set; }
    public string? Currency { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string Status { get; set; } = "Pending"; // Pending, Confirmed, Failed
}