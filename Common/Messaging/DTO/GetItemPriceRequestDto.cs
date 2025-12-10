namespace Common.Messaging.DTO;

public class GetItemPriceRequestDto
{
    public string ItemSku { get; set; } = default!;
    public string Currency { get; set; } = "USD";
}