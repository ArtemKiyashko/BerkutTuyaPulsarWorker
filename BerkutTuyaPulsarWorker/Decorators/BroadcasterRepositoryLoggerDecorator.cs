using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace BerkutTuyaPulsarWorker.Decorators;

public class BroadcasterRepositoryLoggerDecorator : IBroadcasterRepository
{
    private readonly ILogger<BroadcasterRepositoryLoggerDecorator> _logger;
    private readonly TelemetryClient _telemetryClient;
    private readonly IBroadcasterRepository _decoratee;

    public BroadcasterRepositoryLoggerDecorator(
        ILogger<BroadcasterRepositoryLoggerDecorator> logger, 
        TelemetryClient telemetryClient,
        IBroadcasterRepository decoratee)
    {
        _logger = logger;
        _telemetryClient = telemetryClient;
        _decoratee = decoratee;
    }

    public Task PublishDeviceDataAsync(DeviceData deviceData)
    {
        using(_telemetryClient.StartOperation<RequestTelemetry>("PublishDeviceDataAsync"))
        {
            return _decoratee.PublishDeviceDataAsync(deviceData);
        }
    }
}
