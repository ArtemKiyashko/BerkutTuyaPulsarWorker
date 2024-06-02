using BerkutTuyaPulsarWorker.Interfaces;
using BerkutTuyaPulsarWorker.Models;

namespace BerkutTuyaPulsarWorker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly ITuyaWebSocket _webSocket;

    public Worker(ILogger<Worker> logger, ITuyaWebSocket webSocket)
    {
        _logger = logger;
        _webSocket = webSocket;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await _webSocket.ConnectAsync(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            var message = await _webSocket.GetMessageAsync<DeviceData>();
            _logger.LogInformation($"Message ID received: {message.MessageId}");

            //can re-read same message before acknowledge it!
            await _webSocket.AcknowledgeMessageAsync(message.MessageId);
        }
    }
}
