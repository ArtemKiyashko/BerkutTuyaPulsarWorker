namespace BerkutTuyaPulsarWorker.Options;

public class WebSocketOptions
{
    public required string ServerUrl { get; set; } = "wss://mqe.tuyaeu.com:8285/";
    public required string AccessId { get; set; }
    public required string AccessKey { get; set; }
    public required string MqEnv { get; set; } = "event";
    public required string SubscriptionName { get; set; }
    public string? QueryParams { get; set; } = "?ackTimeoutMillis=3000";
    public double PingIntervalSeconds { get; set; } = 30;

}
