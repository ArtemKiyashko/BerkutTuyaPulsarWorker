using System.Text.Json.Serialization;

namespace BerkutTuyaPulsarWorker;


public class MessageContent
{
    [JsonPropertyName("data")]
    public string Data { get; set; }

    [JsonPropertyName("protocol")]
    public int Protocol { get; set; }

    [JsonPropertyName("pv")]
    public string Pv { get; set; }

    [JsonPropertyName("sign")]
    public string Signature { get; set; }

    [JsonPropertyName("t")]
    public long Timestamp { get; set; }
}