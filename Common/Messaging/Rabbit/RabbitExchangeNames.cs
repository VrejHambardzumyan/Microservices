namespace Common.Messaging.Rabbit;

public static class RabbitExchangeNames
{
    public const string CatalogExchange = "catalog.exchange";
    public const string EnrollmentExchange = "enrollment.exchange";

    public const string PriceRequestsQueue = "catalog.price.requests";
    public const string PriceResponsesQueue = "enrollment.price.responses";
}