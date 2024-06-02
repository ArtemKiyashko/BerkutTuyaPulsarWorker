using System.Text.Json.Serialization;

namespace BerkutTuyaPulsarWorker.Models;

public class Message<T>
{
    [JsonPropertyName("messageId")]
    public string MessageId { get; set; }

    [JsonPropertyName("payload")]
    public string Payload { get; set; }

    [JsonPropertyName("properties")]
    public object Properties { get; set; }

    [JsonPropertyName("publishTime")]
    public DateTimeOffset PublishTime { get; set; }

    [JsonPropertyName("redeliveryCount")]
    public int RedeliveryCount { get; set; }

    [JsonPropertyName("key")]
    public string Key { get; set; }
}
