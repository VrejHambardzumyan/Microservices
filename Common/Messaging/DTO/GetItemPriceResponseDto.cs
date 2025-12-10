namespace Common.Messaging.DTO;

public class GetItemPriceResponseDto
{
    public string ItemSku { get; set; } = default!;
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "USD";
    public bool Found { get; set; }
    public string? Error { get; set; }
}