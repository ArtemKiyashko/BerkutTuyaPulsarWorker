namespace BerkutTuyaPulsarWorker.Options;

public class BroadcasterOptions
{
    public required Uri BaseUrl { get; set; }
    public string? ApiKey { get; set; }
    public string? AuthHeaderName { get; set; }
    public required string PostDeviceStatusEndpoint { get; set; }
}
