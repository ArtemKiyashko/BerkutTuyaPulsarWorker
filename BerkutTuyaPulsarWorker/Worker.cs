using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Models;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace BerkutTuyaPulsarWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITuyaWebSocket _webSocket;
    private readonly IBroadcasterRepository _broadcasterRepository;

    public Worker(ILogger<Worker> logger, ITuyaWebSocket webSocket, IBroadcasterRepository broadcasterRepository)
    {
        _logger = logger;
        _webSocket = webSocket;
        _broadcasterRepository = broadcasterRepository;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _webSocket.ConnectAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var message = await _webSocket.GetMessageAsync<DeviceData>();
                _logger.LogInformation($"Message ID received: {message?.MessageId}");
                await _broadcasterRepository.PublishDeviceDataAsync(message?.Result);

                //can re-read same message before acknowledge it!
                //await _webSocket.AcknowledgeMessageAsync(message.MessageId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error processing device message");
            }
        }
    }
}
