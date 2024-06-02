using System.Text.Json.Serialization;

namespace BerkutTuyaPulsarWorker;

public class AcknowledgeMessage
{
    [JsonPropertyName("messageId")]
    public string MessageId { get; set; }
}
