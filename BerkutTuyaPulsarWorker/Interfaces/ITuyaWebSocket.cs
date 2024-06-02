using BerkutTuyaPulsarWorker.Models;

namespace BerkutTuyaPulsarWorker.Interfaces;

public interface ITuyaWebSocket
{
    Task ConnectAsync(CancellationToken cancellationToken);
    Task ConnectAsync();
    Task<Message<T>?> GetMessageAsync<T>();
    Task AcknowledgeMessageAsync(string messageId);
}
