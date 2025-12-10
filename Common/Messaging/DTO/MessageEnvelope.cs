namespace Common.Messaging.DTO;

public class MessageEnvelope<T>
{
    public Guid CorrelationId { get; set; } = Guid.NewGuid();
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public string MessageType { get; set; } = typeof(T).Name;
    public T Payload { get; set; } = default!;
}