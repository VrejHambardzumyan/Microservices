namespace CatalogService.Domain.Entities;

public class Item
{
    public Guid Id { get; set; }
    public string Sku { get; set; } = default!;
    public string Name { get; set; } = default!;
    public bool IsActive { get; set; } = true;

    public ICollection<Price> Prices { get; set; } = new List<Price>();
}