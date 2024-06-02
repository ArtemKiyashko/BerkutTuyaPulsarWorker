using BerkutTuyaPulsarWorker.Models;

namespace BerkutTuyaPulsarWorker.Interfaces;

public interface ITuyaWebSocket
{
    Task ConnectAsync(CancellationToken cancellationToken);
    Task ConnectAsync();
    Task<T?> GetMessageAsync<T>();
}
