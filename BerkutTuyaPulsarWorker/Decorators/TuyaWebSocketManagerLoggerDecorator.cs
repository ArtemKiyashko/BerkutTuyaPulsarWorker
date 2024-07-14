using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace BerkutTuyaPulsarWorker.Decorators;

public class TuyaWebSocketManagerLoggerDecorator : ITuyaWebSocket
{
    private readonly ILogger<TuyaWebSocketManagerLoggerDecorator> _logger;
    private readonly TelemetryClient _telemetryClient;
    private readonly ITuyaWebSocket _decoratee;

    public TuyaWebSocketManagerLoggerDecorator(
        ILogger<TuyaWebSocketManagerLoggerDecorator> logger, 
        TelemetryClient telemetryClient,
        ITuyaWebSocket decoratee)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
        _decoratee = decoratee;
    }

    public Task AcknowledgeMessageAsync(string messageId)
    {
        using(_telemetryClient.StartOperation<RequestTelemetry>("AcknowledgeMessageAsync"))
        {
            return _decoratee.AcknowledgeMessageAsync(messageId);
        }
    }

    public Task ConnectAsync(CancellationToken cancellationToken) => _decoratee.ConnectAsync(cancellationToken);

    public Task ConnectAsync() => _decoratee.ConnectAsync();

    public Task<Message<T>?> GetMessageAsync<T>()
    {
        using(_telemetryClient.StartOperation<RequestTelemetry>("GetMessageAsync"))
        {
            return _decoratee.GetMessageAsync<T>();
        }
    }
}
