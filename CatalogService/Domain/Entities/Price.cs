namespace CatalogService.Domain.Entities;

public class Price
{
    public Guid Id { get; set; }
    public Guid ItemId { get; set; }
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public DateTime EffectiveFrom { get; set; } = DateTime.UtcNow;
    public DateTime? EffectiveTo { get; set; }

    public Item? Item { get; set; }
}